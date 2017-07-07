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
        
        public void Execute(IJobExecutionContext context)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            var jobData = context.JobDetail.JobDataMap;
            var settings = (Settings)jobData["settings"];

            if (!settings.IsEmpty() && !IsWorking)
            {
                string dt = "";
                if (context.Trigger.GetNextFireTimeUtc().HasValue)
                {
                    dt = context.Trigger.GetNextFireTimeUtc().Value.ToLocalTime().ToString("d MMMM yyyy, HH:mm:ss");
                }

                var nextTimeDesc = string.IsNullOrWhiteSpace(dt) ? "" : $" Next at: {dt}";

                try
                {
                    SyncJob.ChangeMainStatusCallback("Synchonization...");
                    var providerA = new FileSyncProvider(settings.ReplicaIdFolderA, settings.FolderA);
                    var providerB = new FileSyncProvider(settings.ReplicaIdFolderB, settings.FolderB);

                    IsWorking = true;
                    logger.Debug("doSync next...");

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
                    syncAgent.Synchronize();
                    
                    SyncJob.ChangeMainStatusCallback($"Up to date.{nextTimeDesc}");
                }
                catch (Exception ex)
                {
                    SyncJob.ChangeMainStatusCallback($"Synchronization error!{nextTimeDesc}");
                    logger.Error(ex);
                }
                finally
                {
                    IsWorking = false;
                }
            }
        }
    }
}
