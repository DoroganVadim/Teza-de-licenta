using DoctorApplication.Models;
using DoctorApplication.Models.Home;
using DoctorApplication.Models.Logs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorApplication.Controllers
{
    [Authorize(Roles = "admin")]
    public class LogsController : Controller
    {
        DoctorAppDbContext context;
        public LogsController(DoctorAppDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;
            var count = await context.logEvents.CountAsync();
            var items = await context.logEvents.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            LogsModel viewModel = new LogsModel
            {
                pageViewModel = pageViewModel,
                logs = items
            };
            return View(viewModel);
        }
    }
}
