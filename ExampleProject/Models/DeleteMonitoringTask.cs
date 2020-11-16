using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleProject.Models
{
    public class DeleteMonitoringTask
    {
        public int MonitoringId { get; set; }
        public string RecurringJobId { get; set; }
        public string Message { get; set; }
    }
}
