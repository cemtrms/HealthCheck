using ExampleProject.Data.UrlModels;
using ExampleProject.Map;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleProject.Data
{
    public class DbDataContext:DbContext
    {
        public DbDataContext(DbContextOptions<DbDataContext> options):base(options)
        {

        }

        public DbSet<MonitoringTask> MonitoringTasks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MonitoringTaskMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
