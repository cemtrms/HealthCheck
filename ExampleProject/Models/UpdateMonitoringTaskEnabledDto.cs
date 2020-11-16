using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleProject.Models
{
    public class UpdateMonitoringTaskEnabledDto
    {
        public int MonitoringId { get; set; }
        public string RecurringJobId { get; set; }
        public bool Enabled { get; set; }
        public string Message { get; set; }
    }
}
