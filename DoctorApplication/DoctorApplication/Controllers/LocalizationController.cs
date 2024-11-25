using DoctorApplication.Models;
using DoctorApplication.Models.DbEntities;
using DoctorApplication.Models.Home;
using DoctorApplication.Models.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;

namespace DoctorApplication.Controllers
{
    [Authorize(Roles = "admin")]
    public class LocalizationController : Controller
    {
        DoctorAppDbContext context;
        public LocalizationController(DoctorAppDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;
            List<IEnumerable<Resource>> resources = new List<IEnumerable<Resource>>();
            var en = context.resources.Where(x => x.culture.name == "en").ToList();
            var ro = context.resources.Where(x => x.culture.name == "ro").ToList();
            var langs = ro.Concat(en).GroupBy(x => x.key).Select(x => new LangClass
            {
                key = x.Key.ToString(),
                langRo = x.ElementAt(0).value?.ToString(),
                langEn = x.ElementAt(1).value?.ToString()
            }).OrderBy(l => l.key).ToList();
            var count = langs.Count();
            var items = langs.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            LocalizationViewModel data = new LocalizationViewModel
            {
                langs = items,
                pageViewModel = pageViewModel
            };
            return View(data);
        }
        public IActionResult AddLocalization()
        {
            return View("LocalizationAddMod", new LangClass());
        }

        public IActionResult EditLocalization(string key, string langEn, string langRo)
        {
            if (key is null)
                return RedirectToAction("AddLocalization");

            if (context.resources.Any(r => r.key == key) == false)
                return RedirectToAction("AddLocalization");

            return View("LocalizationAddMod", new LangClass { key = key, langEn = langEn, langRo = langRo });
        }

        public IActionResult AddMod(LangClass model, string oldKey, int page)
        {
            if (oldKey is null)
            {
                if (model.langEn is null || model.langRo is null) return RedirectToAction("AddLocalization");
                if (context.resources.Any(r => r.key == model.key))
                {
                    context.logEvents.Add(LogEvent.createLog(
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "Added Localization/key='" + model.key + "'",
                    context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                    "Error"
                ));
                    context.SaveChanges();
                    return RedirectToAction("AddLocalization");
                }
                Resource resource = new Resource
                {
                    key = model.key,
                    value = model.langEn,
                    culture = context.cultures.FirstOrDefault(c => c.name == "en")
                };
                context.Add(resource);
                resource = new Resource
                {
                    key = model.key,
                    value = model.langRo,
                    culture = context.cultures.FirstOrDefault(c => c.name == "ro")
                };
                context.Add(resource);
                context.logEvents.Add(LogEvent.createLog(
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "Added Localization/key = '" + model.key + "'",
                    context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                    "Succes"
                ));
                context.SaveChanges();
            }
            else
            {
                var res = context.resources.FirstOrDefault(d => d.key == oldKey && d.culture == context.cultures.FirstOrDefault(c => c.name == "en"));
                if (res is null)
                {
                    context.logEvents.Add(LogEvent.createLog(
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "Modified Localization/key = '" + model.key + "'",
                    context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                    "Error"
                ));
                    context.SaveChanges();
                    return RedirectToAction("AddLocalization");
                }
                res.value = model.langEn;
                res.key = model.key;

                res = context.resources.FirstOrDefault(d => d.key == oldKey && d.culture == context.cultures.FirstOrDefault(c => c.name == "ro"));
                if (res is null)
                    return RedirectToAction("AddLocalization");
                res.value = model.langRo;
                res.key = model.key;
                context.logEvents.Add(LogEvent.createLog(
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "Modified Localization/key = '" + model.key + "'",
                    context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                    "Succes"
                ));
                context.SaveChanges();

            }
            return RedirectToAction("Index", new { page = page });
        }

        public IActionResult Back(int page)
        {
            if (page < 1) page = 1;
            return RedirectToAction("Index", new { page = page });
        }
    }
}
