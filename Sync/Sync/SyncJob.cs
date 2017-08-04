using System;
using Quartz;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Files;
using NLog;

namespace Sync
{
    internal class SyncJob : IJob
    {
        public static bool IsWorking { get; set; }

        public static ChangeMainStatusDelegate ChangeMainStatusCallback;
        private readonly Logger _logger;
        private Settings _settings;

        public SyncJob()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public static string GetDescriptionWithNextTrigger(string description, ITrigger trigger)
        {
            var nextTrigger = "";

            if (trigger.GetNextFireTimeUtc().HasValue) {
                var nextFireTimeUtc = trigger.GetNextFireTimeUtc();

                if (nextFireTimeUtc != null)
                    nextTrigger = nextFireTimeUtc.Value.ToLocalTime().ToString("d MMMM yyyy, HH:mm:ss");
            }

            nextTrigger = string.IsNullOrWhiteSpace(nextTrigger) ? "" : $" Next time run at: {nextTrigger}";

            return $"{description}{nextTrigger}";
        }

        public void Execute(IJobExecutionContext context)
        {
            var jobData = context.JobDetail.JobDataMap;
            _settings = (Settings)jobData["settings"];

            if (_settings.IsEmpty() || IsWorking) return;
            
            try {
                var info = "Start synchonization...";
                ChangeMainStatusCallback(info);
                _logger.Info(info);

                var providerA = new FileSyncProvider(_settings.ReplicaIdFolderA, _settings.FolderA);
                var providerB = new FileSyncProvider(_settings.ReplicaIdFolderB, _settings.FolderB);

                IsWorking = true;

                providerA.DetectChanges();
                providerB.DetectChanges();

                providerA.ApplyingChange += providerA_ApplyingChange;
                providerA.SkippedChange += providerA_SkippedChange;
                providerA.CopyingFile += providerA_CopyingFile;

                providerB.ApplyingChange += providerB_ApplyingChange;
                providerB.SkippedChange += providerB_SkippedChange;
                providerB.CopyingFile += providerB_CopyingFile;

                var syncAgent = new SyncOrchestrator {
                    LocalProvider = providerA,
                    RemoteProvider = providerB,
                    Direction = _settings.Direction == Direction.UploadAndDownload ? SyncDirectionOrder.UploadAndDownload :
                        _settings.Direction == Direction.Upload ? SyncDirectionOrder.Upload :
                            SyncDirectionOrder.Download
                };
                syncAgent.SessionProgress += syncAgent_SessionProgress;
                syncAgent.StateChanged += syncAgent_StateChanged;
                syncAgent.Synchronize();

                info = GetDescriptionWithNextTrigger($"{syncAgent.State}.", context.Trigger);

                ChangeMainStatusCallback(info);
                _logger.Info(info);
            } catch (Exception ex) {
                var info = GetDescriptionWithNextTrigger("Synchronization error!", context.Trigger);

                ChangeMainStatusCallback(info);
                _logger.Info(info);
                _logger.Error(ex);
            } finally {
                IsWorking = false;
            }
        }

        private void providerA_CopyingFile(object sender, CopyingFileEventArgs e)
        {
            ChangeMainStatusCallback($"Copying: {e.FilePath} - {e.PercentCopied}%.");
        }

        private void providerA_SkippedChange(object sender, SkippedChangeEventArgs e)
        {
            var info = $"Skipped change: {e.CurrentFilePath}, reason: {e.SkipReason}.";
            ChangeMainStatusCallback(info);
            _logger.Info(info);
        }

        private void providerB_CopyingFile(object sender, CopyingFileEventArgs e)
        {
            ChangeMainStatusCallback($"Copying: {e.FilePath} - {e.PercentCopied}%.");
        }

        private void providerB_SkippedChange(object sender, SkippedChangeEventArgs e)
        {
            var info = $"Skipped change: {e.CurrentFilePath}, reason: {e.SkipReason}.";
            ChangeMainStatusCallback(info);
            _logger.Info(info);
        }

        private void providerB_ApplyingChange(object sender, ApplyingChangeEventArgs e)
        {
            if (e.ChangeType != ChangeType.Delete) return;

            e.SkipChange = _settings?.SkipDeleteInFolderB ?? false;

            if (_settings != null && !_settings.SkipDeleteInFolderB) return;

            if (_settings == null) return;

            var pathToFileB = System.IO.Path.Combine(_settings.FolderB, e.CurrentFileData.RelativePath);

            if (!System.IO.File.Exists(pathToFileB)) return;

            var nameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(e.CurrentFileData.Name);
            var nameExt = System.IO.Path.GetExtension(e.CurrentFileData.Name);
            var backupName = $"{nameWithoutExt}_{DateTime.Now:yyyyMMddHHmmss}{nameExt}";
            var relativeFolder = e.CurrentFileData.RelativePath;

            relativeFolder = relativeFolder.Replace(e.CurrentFileData.Name, "");

            var relativePathToBackupFileB = System.IO.Path.Combine(Settings.BackupFolderName, DataAccess.Folder.B.ToString(), relativeFolder, backupName);
            var pathToBackupFileB = System.IO.Path.Combine(_settings.FolderB, relativePathToBackupFileB);

            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(pathToBackupFileB))) {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(pathToBackupFileB));
            }

            System.IO.File.Move(pathToFileB, pathToBackupFileB);
            
            var backup = new DataAccess.Backup {
                CreatedAt = DateTime.Now,
                Folder = DataAccess.Folder.B,
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
            
            var pathToFileA = System.IO.Path.Combine(_settings.FolderA, e.CurrentFileData.RelativePath);

            if (!System.IO.File.Exists(pathToFileA)) return;

            var nameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(e.CurrentFileData.Name);
            var nameExt = System.IO.Path.GetExtension(e.CurrentFileData.Name);
            var backupName = $"{nameWithoutExt}_{DateTime.Now:yyyyMMddHHmmss}{nameExt}";
            var relativeFolder = e.CurrentFileData.RelativePath;

            relativeFolder = relativeFolder.Replace(e.CurrentFileData.Name, "");

            var relativePathToBackupFileA = System.IO.Path.Combine(Settings.BackupFolderName, DataAccess.Folder.A.ToString(), relativeFolder, backupName);
            var pathToBackupFileA = System.IO.Path.Combine(_settings.FolderA, relativePathToBackupFileA);

            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(pathToBackupFileA))) {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(pathToBackupFileA));
            }

            System.IO.File.Move(pathToFileA, pathToBackupFileA);
                        
            var backup = new DataAccess.Backup {
                CreatedAt = DateTime.Now,
                Folder = DataAccess.Folder.A,
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

            switch (e.NewState) {
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
