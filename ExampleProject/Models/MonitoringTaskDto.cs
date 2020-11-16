using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleProject.Models
{
    public class MonitoringTaskDto
    {
        public int Id { get; set; }
        public string RecurringJobId { get; set; }
        public string Url { get; set; }
        public bool Enabled { get; set; }
        public int IntervalSeconds { get; set; }
        public string Name { get; set; }
    }
}
