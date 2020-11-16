using ExampleProject.Data;
using ExampleProject.Models;
using ExampleProject.Services;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;

namespace ExampleProject.Controllers
{
    [Authorize]
    public class UrlController : Controller
    {
        private readonly ILogger<UrlController> _logger;
        private readonly DbDataContext _context;
        private readonly IUserService _userService;

        private readonly IMonitoringTaskService _service;

        public UrlController(ILogger<UrlController> logger, DbDataContext context, IMonitoringTaskService service)
        {
            _logger = logger;
            _context = context;
            _service = service;
        }

        public IActionResult Index(string filter, int? currentPage)
        {
            var urls = _service.GetList(filter, pageSize: 10, currentPage: currentPage ?? 1);

            return View(urls);
        }

        public ActionResult Create()
        {
            return PartialView("_InsertMonitoring", new MonitoringTaskDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MonitoringTaskDto dto)
        {

            _service.AddMonitoringTask(dto);
            return RedirectToAction(nameof(Index));
        }

        public ActionResult MonitorTaskEnabled(int id)
        {
            UpdateMonitoringTaskEnabledDto monitoringTask = _service.GetMonitoringTaskForEnabled(id);
            return PartialView("_UpdateMonitoringEnabled", monitoringTask);
        }
        public ActionResult MonitorTaskForDelete(int id)
        {
            var monitoringTask = _service.GetMonitoringTaskForDelete(id);
            return PartialView("_DeleteInfo", monitoringTask);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTask(DeleteMonitoringTask monitoringTask)
        {
            _service.DeleteMonitoringTask(monitoringTask.MonitoringId);
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EnableMonitoringTask(int monitoringId)
        {
            _service.EnableMonitoringTask(monitoringId);

            // throw new Exception(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>     ASDF EnableMonitoringTask");

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DisableMonitorTask(int monitoringId)
        {
            _service.DisableMonitoringTask(monitoringId);

            //  throw new Exception(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>     ASDF DisableMonitorTask");

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult GetGeneralInfo(int id)
        {
            UpdateMonitoringGeneralInfoDto monitoringGeneralInfoDto = _service.GetMonitoringTaskGeneralInfo(id);
            return PartialView("_UpdateMonitoringGeneralInfo", monitoringGeneralInfoDto);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateMonitorTaskGeneralInfo(UpdateMonitoringGeneralInfoDto dto)
        {

            _service.UpdateMonitoringTaskGeneralInfo(dto);

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            _service.DeleteMonitoringTask(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
