using ExampleProject.Controllers;
using ExampleProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace ExampleProject.Services
{
    public static class ChecUrlJob
    {
        //public static void Check()
        //{
        //    using (var scope = GlobalConfiguration.Configuration.DependencyResolver.BeginScope())
        //    {
        //        var missionService = (ChecUrlService)scope.GetService(typeof(ChecUrlService));
        //        var mission = missionService.GetById(id);
        //        if (mission == null) return;
        //        if (mission.Status == 0)
        //            missionService.CloseMisssion(id, 2, "10 ar dakika aralıklarla 3 defa  arandı fakat müşteriye ulaşılamadı,mission kapatıldı");

        //    }
        //}
    }

    public class CheckUrlService : ICheckUrlService
    {
        private readonly ILogger<UrlController> _logger;
        private readonly DbDataContext _context;
        private readonly INotifacationService _notifacationService;
        private readonly IUserService _userService;

        public CheckUrlService(ILogger<UrlController> logger, DbDataContext context, INotifacationService notifacationService, IUserService userService)
        {
            _logger = logger;
            _context = context;
            _notifacationService = notifacationService;
            _userService = userService;
        }

     

        public async Task<HttpResponseMessage> Check(int monitoringTaskId)
        {
         
            Debug.WriteLine("Check Job {0} {1}", monitoringTaskId, DateTime.Now);

            var monitoringTask = _context.MonitoringTasks.Find(monitoringTaskId);
            Debug.WriteLine("\t\t {0}", monitoringTask.Url);
            Debug.WriteLine("");

            var httpClient = new HttpClient();

            var active = await _context.MonitoringTasks.SingleAsync(x => x.Id == monitoringTaskId && x.IsActive && x.Enabled);
            if (active != null)
            {

                var httpResponseMessage = await httpClient.GetAsync(active.Url);
                
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return null;//return something or do something
                }
                else
                {
                    StringBuilder result = new StringBuilder();
                    var data = httpResponseMessage.StatusCode.GetHashCode();
                    if (100 <= data && data <= 199)
                        result =result.Append("Informational responses  " + data) ;
                    if (300 <= data && data <= 399)
                        result =  result.Append("Redirectsstatus, Code :" + data);
                    if (400 <= data && data <= 499)
                        result =  result.Append("Client errors  , Code  :" + data);
                    if (500 <= data && data <=599 )
                        result =  result.Append("Server errors , Code :" + data);
                    result.Append(" " + active.Url);
                    AspNetCoreSerilog.Logging.Concrete.LoggerFactory.DatabaseLogManager().Information("{@message}", result.ToString());
                    AspNetCoreSerilog.Logging.Concrete.LoggerFactory.FileLogManager().Information("test");
                    //mail için Environment bilgilerini girmeniz gerekli,kendi şifremi gizlediğimden atmayacaktır :)
                    _notifacationService.SendeMail(result.ToString(),_userService.GetEmailByUserId(monitoringTask.UserId));
                   
                }


            }

            else
            {

            }
            return null;//return something or do something
        }


    }
}



