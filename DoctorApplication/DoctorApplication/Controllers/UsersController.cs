using DoctorApplication.Models;
using DoctorApplication.Models.DbEntities;
using DoctorApplication.Models.Home;
using DoctorApplication.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorApplication.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        DoctorAppDbContext context;
        public UsersController(DoctorAppDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;
            var count = await context.accounts.CountAsync();
            var items = await context.accounts.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            UsersModel viewModel = new UsersModel
            {
                pageViewModel = pageViewModel,
                users = items
            };
            return View(viewModel);
        }

        public IActionResult EditVerification(int idUser, int page)
        {
            if (idUser is 0)
                return RedirectToAction("Index");
            if (context.accounts.Any(d => d.id == idUser) == false)
                return RedirectToAction("Index");
            var user = context.accounts.FirstOrDefault(d => d.id == idUser);
            if (user.verified) user.verified = false;
            else user.verified = true;

            context.logEvents.Add(LogEvent.createLog(
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                "Edited User Verification/id = " + user.id,
                context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                "Succes"
            ));
            context.SaveChanges();
            return RedirectToAction("Index", new { page = page });
        }

        public IActionResult GrantAdmin(int idUser, int page)
        {
            if (idUser is 0)
                return RedirectToAction("Index");
            if (context.accounts.Any(d => d.id == idUser) == false)
                return RedirectToAction("Index");
            var user = context.accounts.FirstOrDefault(d => d.id == idUser);
            user.role = 1;
            context.logEvents.Add(LogEvent.createLog(
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                "Edited User/Granted Admin/id = " + user.email,
                context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                "Succes"
            ));
            context.SaveChanges();
            return RedirectToAction("Index", new { page = page });
        }

        public IActionResult TakeAdmin(int idUser, int page)
        {
            if (idUser is 0)
                return RedirectToAction("Index");
            if (context.accounts.Any(d => d.id == idUser) == false)
                return RedirectToAction("Index");
            var user = context.accounts.FirstOrDefault(d => d.id == idUser);
            user.role = 0;
            context.logEvents.Add(LogEvent.createLog(
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                "Edited User/Removed Admin/id = " + user.email,
                context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                "Succes"
            ));
            context.SaveChanges();
            return RedirectToAction("Index", new { page = page });
        }

        public IActionResult Back(int page)
        {
            if (page < 1) page = 1;
            return RedirectToAction("Index", new { page = page });
        }
    }
}
