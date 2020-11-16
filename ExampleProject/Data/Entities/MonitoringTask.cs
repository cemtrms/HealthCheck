using ExampleProject.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleProject.Data.UrlModels // Entites
{
    public class MonitoringTask : Entity<int> // MonitoringTask
    {

        // bu alandan filtrele
        public string UserId { get; set; }
        
        public string RecurringJobId { get; set; }
        
        public string Name { get; set; }
        
        public string Url { get;protected set; }

        public int IntervalSeconds { get;protected set; }

        public bool Enabled { get;protected set; }



        public MonitoringTask()
        {

        }

        public MonitoringTask(string userId, string recurringJobId, string name, string url, int intervalSeconds, bool enabled)
        {
            UserId = userId;
            RecurringJobId = recurringJobId; 
            Name = name;
            Url = url;
            IntervalSeconds = intervalSeconds;
            Enabled = enabled;
        }

        public void Enable()
        {
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;
        }

        public void UpdateGeneralInfo(string name, string url, int intervalSeconds)
        {
            Name = name;
            Url = url;
            IntervalSeconds = intervalSeconds;
        }
    }
}
