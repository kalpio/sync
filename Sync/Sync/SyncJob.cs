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

        public void Execute(IJobExecutionContext context)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            var jobData = context.JobDetail.JobDataMap;
            var settings = (Settings)jobData["settings"];

            if (!settings.IsEmpty() && !IsWorking)
            {
                try
                {
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
                }
                catch (Exception ex)
                {
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
