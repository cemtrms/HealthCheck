using ExampleProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleProject.Services
{
    public interface IMonitoringTaskService
    {
        void EnableMonitoringTask(int monitoringId);
        void DisableMonitoringTask(int monitoringId);
        void UpdateMonitoringTaskGeneralInfo( UpdateMonitoringGeneralInfoDto dto);
        void AddMonitoringTask(MonitoringTaskDto dto);
        void DeleteMonitoringTask(int id);
        DeleteMonitoringTask GetMonitoringTaskForDelete(int id);
        PagingModel<MonitoringTaskDto> GetList(string filter, int pageSize, int currentPage);
        UpdateMonitoringGeneralInfoDto GetMonitoringTaskGeneralInfo(int id);
        UpdateMonitoringTaskEnabledDto GetMonitoringTaskForEnabled(int id);
    }
}
