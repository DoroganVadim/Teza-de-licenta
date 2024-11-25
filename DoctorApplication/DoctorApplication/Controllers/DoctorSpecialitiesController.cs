using DoctorApplication.Models;
using DoctorApplication.Models.Account;
using DoctorApplication.Models.DbEntities;
using DoctorApplication.Models.DoctorSpecialities;
using DoctorApplication.Models.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorApplication.Controllers
{
    [Authorize(Roles = "admin")]
    public class DoctorSpecialitiesController : Controller
    {
        DoctorAppDbContext context;
        public DoctorSpecialitiesController(DoctorAppDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;
            var count = await context.doctorSpecialities.CountAsync();
            var items = await context.doctorSpecialities.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            DoctorSpecialitiesViewModel data = new DoctorSpecialitiesViewModel
            {
                docSpecs = items,
                pageViewModel = pageViewModel
            };
            return View(data);
        }

        public IActionResult AddDoctorType()
        {
            return View("DoctorSpecialitiesAddMod", new DoctorSpecialitie());
        }

        public IActionResult EditDoctorType(int idDocType, int page)
        {
            if (idDocType is 0)
                return RedirectToAction("AddDoctorType");

            var type = context.doctorSpecialities.FirstOrDefault(d => d.id == idDocType);
            if (type is null)
                return RedirectToAction("AddDoctorType");
            return View("DoctorSpecialitiesAddMod", type);
        }

        public IActionResult AddMod(DoctorSpecialitie model, int page = 1)
        {
            if (context.doctorSpecialities.Any(d => d.name == model.name)) return View("DoctorTypeAddMod", model);
            if (model.id is 0)
            {
                if (model.name is null) return RedirectToAction("AddDoctorType");
                DoctorSpecialitie doc = new DoctorSpecialitie { name = model.name, enabled = true };
                context.Add(doc);
                context.logEvents.Add(LogEvent.createLog(
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                "Added Doctor Speciality name = " + doc.name,
                context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                "Succes"
                ));
                context.SaveChanges();
            }
            else
            {
                var type = context.doctorSpecialities.FirstOrDefault(d => d.id == model.id);
                if (type is null)
                    return RedirectToAction("AddDoctor");
                type.name = model.name;
                context.logEvents.Add(LogEvent.createLog(
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                "Modified Doctor Speciality/id = " + type.id,
                context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                "Succes"
                ));
                context.SaveChanges();
            }
            return RedirectToAction("Index", new { page = page });
        }

        public IActionResult Enable(int idType, int page)
        {
            if (idType is 0)
            {
                context.logEvents.Add(LogEvent.createLog(
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "Enabled Doctor Speciality/id = " + idType,
                    context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                    "Error"
                ));
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            if (context.doctorSpecialities.Any(d => d.id == idType) == false)
            {
                context.logEvents.Add(LogEvent.createLog(
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                "Enabled Doctor Speciality/id = " + idType,
                context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                "Error"
                ));
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            var type = context.doctorSpecialities.FirstOrDefault(d => d.id == idType);
            type.enabled = true;
            context.logEvents.Add(LogEvent.createLog(
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                "Enabled Doctor Speciality/id = " + idType,
                context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                "Succes"
                ));
            context.SaveChanges();
            return RedirectToAction("Index", new { page = page });
        }

        public IActionResult Disable(int idType, int page)
        {
            if (idType is 0)
            {
                context.logEvents.Add(LogEvent.createLog(
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                "Disable Doctor Speciality/id = " + idType,
                context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                "Error"
                ));
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            if (context.doctorSpecialities.Any(d => d.id == idType) == false)
            {
                context.logEvents.Add(LogEvent.createLog(
                   HttpContext.Connection.RemoteIpAddress?.ToString(),
                   "Disable Doctor Speciality/id = " + idType,
                   context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                   "Error"
                   ));
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            var type = context.doctorSpecialities.FirstOrDefault(d => d.id == idType);
            type.enabled = false;
            context.logEvents.Add(LogEvent.createLog(
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                "Disable Doctor Speciality/id = " + idType,
                context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                "Error"
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
