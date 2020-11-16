using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleProject.BackGroundJob
{
    public class Recurring_Job
    {
        public Recurring_Job()
        {
            //Recurring job
            RecurringJob.AddOrUpdate(() => ProcessRecurringJob(""), Cron.Hourly);
        }

        public void ProcessRecurringJob(string url)
        {
            Console.WriteLine("I am a Recurring Job !!"+url);
        }
    }
}
