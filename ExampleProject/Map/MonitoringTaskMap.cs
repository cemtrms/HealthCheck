using ExampleProject.Data.UrlModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleProject.Map
{
    public class MonitoringTaskMap : IEntityTypeConfiguration<MonitoringTask>
    {
        public void Configure(EntityTypeBuilder<MonitoringTask> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p=>p.Url).HasMaxLength(300);
            builder.Property(p => p.IsActive).HasDefaultValue(true);
        }
    }
}
