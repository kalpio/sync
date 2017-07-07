using System;
using Quartz;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Files;
using NLog;

namespace Sync
{
    class SyncJob : IJob
    {
        public static bool IsWorking { get; set; }

        public static ChangeMainStatusDelegate ChangeMainStatusCallback;
        Logger logger;

        public SyncJob()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        public static string GetDescriptionWithNextTrigger(string description, ITrigger trigger)
        {
            string nextTrigger = "";

            if (trigger.GetNextFireTimeUtc().HasValue)
            {
                nextTrigger = trigger.GetNextFireTimeUtc().Value.ToLocalTime().ToString("d MMMM yyyy, HH:mm:ss");
            }

            nextTrigger = string.IsNullOrWhiteSpace(nextTrigger) ? "" : $" Next time run at: {nextTrigger}";

            return $"{description}{nextTrigger}";
        }

        public void Execute(IJobExecutionContext context)
        {
            var jobData = context.JobDetail.JobDataMap;
            var settings = (Settings)jobData["settings"];

            if (!settings.IsEmpty() && !IsWorking)
            {
                try
                {
                    var info = "Start synchonization...";
                    ChangeMainStatusCallback(info);
                    logger.Info(info);

                    var providerA = new FileSyncProvider(settings.ReplicaIdFolderA, settings.FolderA);
                    var providerB = new FileSyncProvider(settings.ReplicaIdFolderB, settings.FolderB);

                    IsWorking = true;

                    providerA.DetectChanges();
                    providerB.DetectChanges();

                    var syncAgent = new SyncOrchestrator()
                    {
                        LocalProvider = providerA,
                        RemoteProvider = providerB,
                        Direction = settings.Direction == Direction.UploadAndDownload ? SyncDirectionOrder.UploadAndDownload :
                            settings.Direction == Direction.Upload ? SyncDirectionOrder.Upload :
                            SyncDirectionOrder.Download
                    };
                    syncAgent.SessionProgress += syncAgent_SessionProgress;
                    syncAgent.StateChanged += syncAgent_StateChanged;
                    syncAgent.Synchronize();

                    info = GetDescriptionWithNextTrigger($"{syncAgent.State.ToString()}.", context.Trigger);

                    ChangeMainStatusCallback(info);
                    logger.Info(info);
                }
                catch (Exception ex)
                {
                    var info = GetDescriptionWithNextTrigger("Synchronization error!", context.Trigger);

                    ChangeMainStatusCallback(info);
                    logger.Info(info);
                    logger.Error(ex);
                }
                finally
                {
                    IsWorking = false;
                }
            }
        }

        private void syncAgent_SessionProgress(object sender, SyncStagedProgressEventArgs e)
        {
            var info = $"Synchronization... {e.ReportingProvider.ToString()}. File {e.CompletedWork.ToString("N0")} of {e.TotalWork.ToString("N0")}.";

            ChangeMainStatusCallback(info);
            logger.Info(info);
        }

        private void syncAgent_StateChanged(object sender, SyncOrchestratorStateChangedEventArgs e)
        {
            string info = "";

            switch (e.NewState)
            {
                case SyncOrchestratorState.Ready:
                    info = $"{e.NewState.ToString()}.";
                    break;
                case SyncOrchestratorState.Uploading:
                    info = $"{e.NewState.ToString()}...";
                    break;
                case SyncOrchestratorState.Downloading:
                    info = $"{e.NewState.ToString()}...";
                    break;
                case SyncOrchestratorState.UploadingAndDownloading:
                    info = $"{e.NewState.ToString()}...";
                    break;
                case SyncOrchestratorState.Canceling:
                    info = $"{e.NewState.ToString()}...";
                    break;
                case SyncOrchestratorState.Canceled:
                    info = $"{e.NewState.ToString()}.";
                    break;
                default:
                    info = "No information!";
                    break;
            }
            ChangeMainStatusCallback(info);
            logger.Info(info);
        }
    }
}
