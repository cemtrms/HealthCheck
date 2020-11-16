using ExampleProject.BackGroundJob;
using ExampleProject.Data;
using ExampleProject.Data.UrlModels;
using ExampleProject.Models;
using ExampleProject.UnitOfWork;
using Hangfire;
using System;
using System.Linq;

namespace ExampleProject.Services
{
    public class MonitoringTaskService : IMonitoringTaskService
    {
        private readonly DbDataContext _context;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        public MonitoringTaskService(DbDataContext context, IUserService userService,IUnitOfWork unitOfWork)
        {
            _context = context;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public void EnableMonitoringTask(int monitoringId)
        {
           
                var monitoringTask = _context.MonitoringTasks.Find(monitoringId);

                monitoringTask.Enable();
                AddOrUpdateBackgroundjob(monitoringTask);
            _unitOfWork.SaveChanges();
            
        }

        public void DisableMonitoringTask(int monitoringId)
        {
            
                var monitoringTask = _context.MonitoringTasks.Find(monitoringId);

                monitoringTask.Disable();
                RecurringJob.RemoveIfExists(monitoringTask.RecurringJobId);
            _unitOfWork.SaveChanges();
        }

        public void UpdateMonitoringTaskGeneralInfo(UpdateMonitoringGeneralInfoDto dto)
        {
            var userId = _userService.GetUser()?.Id;
            if (!string.IsNullOrEmpty(userId))
            {
                var task = _context.MonitoringTasks.Single(p => p.Id == dto.Id && p.RecurringJobId == dto.RecurringJobId && p.UserId == userId);
                task.UpdateGeneralInfo(dto.Name, dto.Url, dto.IntervalSeconds);
               
                AddOrUpdateBackgroundjob(task);

                _unitOfWork.SaveChanges();
            }
        }

        public void AddMonitoringTask(MonitoringTaskDto dto)
        {
            var userId = _userService.GetUser()?.Id;
           
                if (!string.IsNullOrEmpty(userId))
                { 
                    var recurringJobId = Guid.NewGuid().ToString();

                    var monitoringTask = new MonitoringTask(
                            userId: userId,
                         recurringJobId: recurringJobId,
                         name: dto.Name,
                         url: dto.Url,
                         intervalSeconds: dto.IntervalSeconds,
                         enabled: dto.Enabled);

                    _context.MonitoringTasks.Add(monitoringTask);
                _unitOfWork.SaveChanges();

                AddOrUpdateBackgroundjob(monitoringTask);
                _unitOfWork.SaveChanges();

            }


        }

        private static void AddOrUpdateBackgroundjob(MonitoringTask monitoringTask)
        {

            RecurringJob.AddOrUpdate<ICheckUrlService>(
            recurringJobId: monitoringTask.RecurringJobId,
            methodCall: s => s.Check(monitoringTask.Id),
            cronExpression: "0/" + monitoringTask.IntervalSeconds + " * * * * *");
        }

        public void DeleteMonitoringTask(int id)
        {
           
            string userId = _userService.GetUser().Id;
            if (!string.IsNullOrEmpty(userId))
            {
               
                    var monitoringTask = _context.MonitoringTasks.Single(p => p.Id == id && p.UserId == userId);
                    monitoringTask.Delete();
                _unitOfWork.SaveChanges();

                // do not inline used in Lambda Expression

                //var job = new Recurring_Job();
                //job.ProcessRecurringJob(urlData.Url);

                RecurringJob.RemoveIfExists(monitoringTask.RecurringJobId);
                _unitOfWork.SaveChanges();
            }
          
        }

        public DeleteMonitoringTask GetMonitoringTaskForDelete(int id)
        {
            var userId = _userService.GetUser().Id;
            var monitoringTask = _context.MonitoringTasks.Single(p => p.Id == id && p.UserId == userId);
            return new DeleteMonitoringTask { MonitoringId = monitoringTask.Id, RecurringJobId = monitoringTask.RecurringJobId, Message = "Are you sure want to delete task ?" };
        }



        public PagingModel<MonitoringTaskDto> GetList(string filter, int pageSize, int currentPage)
        {
            var userId = _userService.GetUser().Id;
            var filteredMonitoringTasks = _context.MonitoringTasks
                .Where(
                    p => p.IsActive && p.UserId == userId && (filter == null
                        || p.Url.Contains(filter)))
                .ToList();

            var productsInPage = filteredMonitoringTasks
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new MonitoringTaskDto { Id = p.Id, RecurringJobId = p.RecurringJobId, Name = p.Name, Url = p.Url, Enabled = p.Enabled, IntervalSeconds = p.IntervalSeconds }).ToList();

            return new PagingModel<MonitoringTaskDto>(filter, filteredMonitoringTasks.Count, currentPage, productsInPage);
        }



        public UpdateMonitoringGeneralInfoDto GetMonitoringTaskGeneralInfo(int id)
        {
            var userId = _userService.GetUser().Id;
            var monitoringTask = _context.MonitoringTasks.Single(p => p.Id == id && p.UserId == userId);
            return new UpdateMonitoringGeneralInfoDto { Id = monitoringTask.Id, RecurringJobId = monitoringTask.RecurringJobId, IntervalSeconds = monitoringTask.IntervalSeconds, Name = monitoringTask.Name };
        }

        public UpdateMonitoringTaskEnabledDto GetMonitoringTaskForEnabled(int id)
        {
            var userId = _userService.GetUser().Id;
            var monitoringTask = _context.MonitoringTasks.Single(p => p.Id == id && p.UserId == userId);
            return new UpdateMonitoringTaskEnabledDto { MonitoringId = monitoringTask.Id, RecurringJobId = monitoringTask.RecurringJobId, Message=monitoringTask.Enabled?"Task will stop.":"Task will run.",Enabled=!monitoringTask.Enabled };
        }
    }
}
