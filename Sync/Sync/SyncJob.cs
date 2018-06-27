using System;
using System.IO;
using System.Timers;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Files;
using NLog;
using Sync.DataAccess;

namespace Sync
{
  internal class SyncJob
  {
    private static bool IsWorking { get; set; }

    public static    ChangeMainStatusDelegate ChangeMainStatusCallback;
    private readonly Logger                   _logger;
    private readonly Settings                 _settings;
    private readonly Timer                    _timer;
    private          DateTime                 _nextTriggerTime;

    public SyncJob(Settings settings)
    {
      _logger          =  LogManager.GetCurrentClassLogger();
      _timer           =  new Timer(1000);
      _settings        =  settings;
      _nextTriggerTime =  _settings.StartAt;
      _timer.Elapsed   += TimerOnElapsed;
    }

    private void TimerOnElapsed(object sender, ElapsedEventArgs e)
    {
      if (e.SignalTime < _nextTriggerTime || IsWorking) return;
      var result = Execute();
      _nextTriggerTime = DateTime.Now.AddMilliseconds(GetMiliseconds());
      var info = GetDescriptionWithNextTrigger($"{result}.");
      ChangeMainStatusCallback(info);
      _logger.Info($"Timer OnElapsed: {info}");
    }

    private string GetDescriptionWithNextTrigger(string description)
    {
      var nextTrigger = _nextTriggerTime.ToString("d MMMM yyyy, HH:mm:ss");
      nextTrigger = string.IsNullOrWhiteSpace(nextTrigger) ? "" : $" Next time run at: {nextTrigger}";

      return $"{description}{nextTrigger}";
    }

    private double GetMiliseconds()
    {
      switch (_settings.IntervalType)
      {
        case IntervalType.Millisecond:
          return _settings.Interval;
        case IntervalType.Second:
          return TimeSpan.FromSeconds(_settings.Interval).TotalMilliseconds;
        case IntervalType.Minute:
          return TimeSpan.FromMinutes(_settings.Interval).TotalMilliseconds;
        case IntervalType.Hour:
          return TimeSpan.FromHours(_settings.Interval).TotalMilliseconds;
        case IntervalType.Day:
          return TimeSpan.FromDays(_settings.Interval).TotalDays;
        case IntervalType.Week:
          return TimeSpan.FromDays(_settings.Interval * 7).TotalMilliseconds;
        case IntervalType.Month:
          return TimeSpan.FromDays(_settings.Interval * 30).TotalMilliseconds;
        case IntervalType.Year:
          return TimeSpan.FromDays(_settings.Interval * 365).TotalMilliseconds;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public void Start()
    {
      _timer.Enabled = true;
      _timer.Start();
    }

    public void Stop()
    {
      _timer.Enabled = false;
      _timer.Stop();
    }

    private string Execute()
    {
      if (_settings.IsEmpty() || IsWorking) return string.Empty;
      IsWorking = true;

      try
      {
        var info = "Start synchonization...";
        ChangeMainStatusCallback(info);
        _logger.Info(info);

        var providerA = new FileSyncProvider(_settings.ReplicaIdFolderA, _settings.FolderA);
        var providerB = new FileSyncProvider(_settings.ReplicaIdFolderB, _settings.FolderB);

        providerA.DetectChanges();
        providerB.DetectChanges();

        providerA.ApplyingChange += providerA_ApplyingChange;
        providerA.SkippedChange  += providerA_SkippedChange;
        providerA.CopyingFile    += providerA_CopyingFile;

        providerB.ApplyingChange += providerB_ApplyingChange;
        providerB.SkippedChange  += providerB_SkippedChange;
        providerB.CopyingFile    += providerB_CopyingFile;

        var syncAgent = new SyncOrchestrator
        {
          LocalProvider  = providerA,
          RemoteProvider = providerB,
          Direction = _settings.Direction == Direction.UploadAndDownload ? SyncDirectionOrder.UploadAndDownload :
            _settings.Direction == Direction.Upload                      ? SyncDirectionOrder.Upload :
                                                                           SyncDirectionOrder.Download
        };
        syncAgent.SessionProgress += syncAgent_SessionProgress;
        syncAgent.StateChanged    += syncAgent_StateChanged;
        syncAgent.Synchronize();

        return syncAgent.State.ToString();
      }
      catch (Exception ex)
      {
        _logger.Error(ex);
        return "Synchronization error!";
      }
      finally
      {
        IsWorking = false;
      }
    }

    private void providerA_CopyingFile(object sender, CopyingFileEventArgs e)
    {
      ChangeMainStatusCallback($"Copying: {e.FilePath} - {e.PercentCopied}%.");
    }

    private void providerA_SkippedChange(object sender, SkippedChangeEventArgs e)
    {
      var path = string.IsNullOrWhiteSpace(e.CurrentFilePath)
        ? e.NewFilePath
        : string.IsNullOrWhiteSpace(e.CurrentFilePath)
          ? ""
          : e.CurrentFilePath;
      var info = $"Skipped change: {path}, reason: {e.SkipReason}.";
      _logger.Info(info);

      if (e.Exception != null) _logger.Error(e);

      ChangeMainStatusCallback(info);
    }

    private void providerB_CopyingFile(object sender, CopyingFileEventArgs e)
    {
      ChangeMainStatusCallback($"Copying: {e.FilePath ?? ""} - {e.PercentCopied}%.");
    }

    private void providerB_SkippedChange(object sender, SkippedChangeEventArgs e)
    {
      var path = string.IsNullOrWhiteSpace(e.CurrentFilePath)
        ? e.NewFilePath
        : string.IsNullOrWhiteSpace(e.CurrentFilePath)
          ? ""
          : e.CurrentFilePath;
      var info = $"Skipped change: {path}. Reason: {e.SkipReason}.";
      _logger.Info(info);

      if (e.Exception != null) _logger.Error(e);

      ChangeMainStatusCallback(info);
    }

    private void providerB_ApplyingChange(object sender, ApplyingChangeEventArgs e)
    {
      if (e.ChangeType != ChangeType.Delete) return;

      e.SkipChange = _settings?.SkipDeleteInFolderB ?? false;

      if (_settings != null && !_settings.SkipDeleteInFolderB) return;

      if (_settings == null) return;

      var pathToFileB = Path.Combine(_settings.FolderB, e.CurrentFileData.RelativePath);

      if (!File.Exists(pathToFileB)) return;

      var nameWithoutExt = Path.GetFileNameWithoutExtension(e.CurrentFileData.Name);
      var nameExt        = Path.GetExtension(e.CurrentFileData.Name);
      var now            = DateTime.Now;
      var backupName     = $"{nameWithoutExt}_{now:yyyyMMdd}_{now:HHmmss}{nameExt}";
      var relativeFolder = e.CurrentFileData.RelativePath;

      relativeFolder = relativeFolder.Replace(e.CurrentFileData.Name, "");

      var relativePathToBackupFileB =
        Path.Combine(Settings.BackupFolderName, Folder.B.ToString(), relativeFolder, backupName);
      var pathToBackupFileB = Path.Combine(_settings.FolderB, relativePathToBackupFileB);

      if (!Directory.Exists(Path.GetDirectoryName(pathToBackupFileB)))
      {
        var path = Path.GetDirectoryName(pathToBackupFileB);

        if (string.IsNullOrWhiteSpace(path))
        {
          var info = $"GetDirectoryName from {pathToBackupFileB}{nameof(pathToBackupFileB)} is Null";
          ChangeMainStatusCallback(info);
          _logger.Error(info);

          return;
        }

        Directory.CreateDirectory(path);
      }

      File.Move(pathToFileB, pathToBackupFileB);

      var backup = new Backup
      {
        CreatedAt    = DateTime.Now,
        Folder       = Folder.B,
        RelativePath = relativePathToBackupFileB
      };

      SyncFactory.Get().SaveOrUpdate(backup);
    }

    private void providerA_ApplyingChange(object sender, ApplyingChangeEventArgs e)
    {
      if (e.ChangeType != ChangeType.Delete) return;

      e.SkipChange = _settings?.SkipDeleteInFolderA ?? false;

      if (_settings != null && !_settings.SkipDeleteInFolderA) return;

      if (_settings == null) return;

      var pathToFileA = Path.Combine(_settings.FolderA, e.CurrentFileData.RelativePath);

      if (!File.Exists(pathToFileA)) return;

      var nameWithoutExt = Path.GetFileNameWithoutExtension(e.CurrentFileData.Name);
      var nameExt        = Path.GetExtension(e.CurrentFileData.Name);
      var backupName     = $"{nameWithoutExt}_{DateTime.Now:yyyyMMddHHmmss}{nameExt}";
      var relativeFolder = e.CurrentFileData.RelativePath;

      relativeFolder = relativeFolder.Replace(e.CurrentFileData.Name, "");

      var relativePathToBackupFileA =
        Path.Combine(Settings.BackupFolderName, Folder.A.ToString(), relativeFolder, backupName);
      var pathToBackupFileA = Path.Combine(_settings.FolderA, relativePathToBackupFileA);

      if (!Directory.Exists(Path.GetDirectoryName(pathToBackupFileA)))
      {
        var path = Path.GetDirectoryName(pathToBackupFileA);

        if (string.IsNullOrWhiteSpace(path))
        {
          var info = $"GetDirectoryName from {pathToBackupFileA}(pathToBackupFileA) is Null";
          ChangeMainStatusCallback(info);
          _logger.Error(info);

          return;
        }

        Directory.CreateDirectory(path);
      }

      File.Move(pathToFileA, pathToBackupFileA);

      var backup = new Backup
      {
        CreatedAt    = DateTime.Now,
        Folder       = Folder.A,
        RelativePath = relativePathToBackupFileA
      };

      SyncFactory.Get().SaveOrUpdate(backup);
    }

    private void syncAgent_SessionProgress(object sender, SyncStagedProgressEventArgs e)
    {
      var info = $"Synchronization... {e.ReportingProvider}. File {e.CompletedWork:N0} of {e.TotalWork:N0}.";

      ChangeMainStatusCallback(info);
      _logger.Info(info);
    }

    private void syncAgent_StateChanged(object sender, SyncOrchestratorStateChangedEventArgs e)
    {
      string info;

      switch (e.NewState)
      {
        case SyncOrchestratorState.Uploading:
        case SyncOrchestratorState.Downloading:
        case SyncOrchestratorState.UploadingAndDownloading:
        case SyncOrchestratorState.Canceling:
          info = $"{e.NewState}...";
          break;
        case SyncOrchestratorState.Ready:
        case SyncOrchestratorState.Canceled:
          info = $"{e.NewState}.";
          break;
        default:
          info = "No information!";
          break;
      }

      ChangeMainStatusCallback(info);
      _logger.Info(info);
    }
  }
}
