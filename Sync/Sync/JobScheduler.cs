using System;
using System.Collections.Specialized;
using Quartz;
using Quartz.Impl;

namespace Sync
{
    internal static class JobScheduler
    {
        private static IScheduler _jobScheduler;
        private static ISchedulerFactory _schedulerFactory;

        public static IScheduler Get()
        {
            if (_jobScheduler == null)
            {
                NameValueCollection properties = new NameValueCollection();
                properties["quartz.scheduler.instanceName"] = "SyncJobs";
                properties["quartz.scheduler.instanceId"] = "SyncJobs";
                properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
                properties["quartz.threadPool.threadCount"] = "10";
                properties["quartz.threadPool.threadPriority"] = "Normal";
                properties["quartz.jobStore.type"] = "Quartz.Simpl.RAMJobStore, Quartz";
                _schedulerFactory = new StdSchedulerFactory(properties);
                _jobScheduler = _schedulerFactory.GetScheduler();
            }
            if (!_jobScheduler.IsStarted || _jobScheduler.IsShutdown || _jobScheduler.InStandbyMode)
            {
                _jobScheduler.Start();
            }
            return _jobScheduler;
        }

        public static bool JobExist(string key, string group)
        {
            var job_key = JobKey.Create(key, group);
            return Get().CheckExists(job_key);
        }
    }
}
