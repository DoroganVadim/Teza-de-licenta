using ClosedXML.Excel;
using DoctorApplication.Classes;
using DoctorApplication.Models;
using DoctorApplication.Models.DbEntities;
using DoctorApplication.Models.Doctor;
using DoctorApplication.Models.Home;
using DoctorApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Data;

namespace DoctorApplication.Controllers
{
    [Authorize(Roles = "admin")]
    public class DoctorController : Controller
    {
        private DoctorAppDbContext context;
        private readonly IEmailTemplateService templateService;
        public DoctorController(DoctorAppDbContext Context, IEmailTemplateService templateService)
        {
            this.context = Context;
            templateService = templateService;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;
            var count = await context.doctors.CountAsync();
            var items = await context.doctors.Include(d => d.specialities).Include(d=>d.account).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var types = context.doctorSpecialities.ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            DoctorViewModel viewModel = new DoctorViewModel
            {
                pageViewModel = pageViewModel,
                doctors = items,
                doctorTypes = types
            };
            return View(viewModel);
        }


        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> Appointments(int idDoc, int page = 1)
        {
            if (idDoc is 0)
            {
                return RedirectToAction("Index");
            }
            int pageSize = 5;
            var count = 0;
            var items = new List<Appointment>();
            try
            {
                count = await context.appointments.Include(a => a.doctor).Where(a => a.doctor.id == idDoc).CountAsync();
                items = await context.appointments.Include(a => a.doctor).Where(a => a.doctor.id == idDoc).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            }catch(Exception e)
            {
                count = 0;
                items = new List<Appointment>();
            }
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            AppointmentsViewModel viewModel = new AppointmentsViewModel
            {
                idDoctor = idDoc,
                pageViewModel = pageViewModel,
                appointments = items
            };
            Doctor doc = context.doctors.Where(d => d.id == idDoc).FirstOrDefault();
            try
            {
                doc.appointmentNum = context.appointments.Include(a => a.doctor).Where(a => a.doctor.id == doc.id).Count();
            }
            catch(Exception e)
            {
                doc.appointmentNum = 0;
            }
            context.SaveChanges();
            return View(viewModel);
        }

        public IActionResult EditDoctor(int idDoc, int page)
        {
            if (idDoc is 0)
                return RedirectToAction("AddDoctor");

            var doctor = context.doctors.Include(d => d.specialities).FirstOrDefault(d => d.id == idDoc);
            if (doctor is null)
                return RedirectToAction("AddDoctor");
            var types = context.doctorSpecialities.Where(d => d.enabled == true);
            return View("DoctorAddMod", new DoctorSpecialityModel { doctor = doctor, doctorSpecialities = types });
        }


        public IActionResult AddMod(DoctorSpecialityModel model, int[] ids, IFormFile file, int page = 1)
        {
            if (model.doctor.id is 0)
            {
                if (model.doctor.surrname is null || model.doctor.name is null || model.doctor.account.email is null || model.doctor.tel is null) return RedirectToAction("AddDoctor");
            }
            var doctor = context.doctors.Include(d => d.specialities).Include(d=>d.account).FirstOrDefault(d => d.id == model.doctor.id);
            var types = context.doctorSpecialities.Where(d => ids.Contains(d.id)).ToList();
            if (doctor is null)
                return RedirectToAction("AddDoctor");
            doctor.surrname = model.doctor.surrname;
            doctor.name = model.doctor.name;
            doctor.account.email = model.doctor.account.email;
            doctor.tel = model.doctor.tel;

            var typesDelDif = doctor.specialities.Except(types).ToList();
            var typesAddDif = types.Except(doctor.specialities).ToList();
            foreach (var type in typesDelDif)
            {
                doctor.specialities.Remove(type);
            }

            foreach (var type in typesAddDif)
            {
                doctor.specialities.Add(type);
            }

            if (file != null) doctor.imageString = ImageStringConvert.ConvertImageToString(file);
            context.logEvents.Add(LogEvent.createLog(
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                "Modified Doctor/Id = " + doctor.id,
                context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                "Succes"
                ));
            context.SaveChanges();
            return RedirectToAction("Index", new { page = page });
        }

        public IActionResult Enable(int idDoc, int page)
        {
            if (idDoc is 0)
            {
                context.logEvents.Add(LogEvent.createLog(
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "Enabled Doctor/id = " + idDoc,
                    context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                    "Error"
                    ));
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            if (context.doctors.Any(d => d.id == idDoc) == false)
            {
                context.logEvents.Add(LogEvent.createLog(
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "Enabled Doctor/id = " + idDoc,
                    context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                    "Error"
                    ));
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            var doc = context.doctors.FirstOrDefault(d => d.id == idDoc);
            doc.activityStatus = true;
            context.logEvents.Add(LogEvent.createLog(
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                "Enabled Doctor/id = " + idDoc,
                context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                "Succes"
                ));
            context.SaveChanges();
            return RedirectToAction("Index", new { page = page });
        }

        public IActionResult Disable(int idDoc, int page)
        {
            if (idDoc is 0)
            {
                context.logEvents.Add(LogEvent.createLog(
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                "Disabled Doctor/id = " + idDoc,
                context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                "Error"
                ));
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            if (context.doctors.Any(d => d.id == idDoc) == false)
            {
                context.logEvents.Add(LogEvent.createLog(
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                "Disabled Doctor/id = " + idDoc,
                context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                "Error"
                ));
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            var doc = context.doctors.FirstOrDefault(d => d.id == idDoc);
            doc.activityStatus = false;
            context.logEvents.Add(LogEvent.createLog(
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                "Disabled Doctor/id = " + idDoc,
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

        public ActionResult SetDateEndLimit(DateTime date)
        {
            return Json(date.ToString("yyyy-MM-dd"));
        }

        public ActionResult SetTimeEndLimit(string time, bool dateCheck)
        {
            List<string> baseTimes = new List<string> {
                "08:00",
                "08:30",
                "09:00",
                "09:30",
                "10:00",
                "10:30",
                "11:00",
                "11:30",
                "12:00",
                "13:00",
                "13:30",
                "14:00",
                "14:30",
                "15:00",
                "15:30",
                "16:00",
                "16:30",
                "17:00"
            };
            if (dateCheck is false)
            {
                return Json(baseTimes);
            }
            List<string> times = new List<string>();
            foreach (string s in baseTimes)
            {
                if (s.CompareTo(time) >= 0) times.Add(s);

            }
            return Json(times);
        }


        [HttpPost]
        public IActionResult Export(DateTime dateStart, DateTime dateEnd, string timeStart, string timeEnd, int idDoc)
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[6] { new DataColumn("Name"),
                                        new DataColumn("Surname"),
                                        new DataColumn("PhoneNumber"),
                                        new DataColumn("Day"),
                                        new DataColumn("Time"),
                                        new DataColumn("Confirmed")
            });

            DateTime totalStart = dateStart + TimeSpan.Parse(timeStart);
            DateTime totalEnd = dateEnd + TimeSpan.Parse(timeEnd);
            var inregs = context.appointments.Include(a=>a.doctor).Where(a => a.doctor.id == idDoc && a.appointmentDate.CompareTo(totalStart) > 0 && a.appointmentDate.CompareTo(totalEnd) < 0).ToList();
            foreach (var inreg in inregs)
            {
                dt.Rows.Add(inreg.surrnamePacient, inreg.namePacient, inreg.tel, inreg.appointmentDate.ToString("dd/MM/yyyy"), inreg.appointmentTime, inreg.confirmedUser);
            }

            context.logEvents.Add(LogEvent.createLog(
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                "Exported Raport",
                context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                "Succes"
                ));
            context.SaveChanges();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Raport.xlsx");
                }
            }
        }
        public IActionResult ChangeVerifiedStatus(int idDoc, int page)
        {
            if (idDoc is 0)
                return RedirectToAction("Index");
            if (context.doctors.Any(d => d.id == idDoc) == false)
                return RedirectToAction("Index");
            var doc = context.doctors.FirstOrDefault(d => d.id == idDoc);
            if (doc.verified) doc.verified = false;
            else doc.verified = true;

            context.logEvents.Add(LogEvent.createLog(
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                "Edited Doctor Verification/id = " + doc.id,
                context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                "Succes"
            ));
            context.SaveChanges();
            return RedirectToAction("Index", new { page = page });
        }

        public async Task<IActionResult> DoctorApplicationConfirmation(int page = 1)
        {
            int pageSize = 5;
            var count = await context.doctors.Where(d => d.activityStatus == true && d.verified == false).CountAsync();
            var items = await context.doctors.Include(d => d.specialities).Skip((page - 1) * pageSize).Where(d => d.verified == false && d.activityStatus == true).Take(pageSize).ToListAsync();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            DoctorApplicationModel viewModel = new DoctorApplicationModel
            {
                pageViewModel = pageViewModel,
                doctors = items,
            };
            return View(viewModel);
        }

        public IActionResult DoctorProfileCheck(int idDoc, int page = 1)
        {
            if (context.doctors.Any(d => d.id == idDoc) is false || idDoc is 0) return RedirectToAction("DoctorApplicationConfirmation");
            @TempData["Page"] = page;
            Doctor model = context.doctors.Where(d => d.id == idDoc).Include(d => d.specialities).FirstOrDefault();
            return View(model);
        }

        public async Task<IActionResult> AcceptDoctor(int idDoc, int page = 1)
        {
            if (context.doctors.Any(d => d.id == idDoc) is false || idDoc is 0) return RedirectToAction("DoctorApplicationConfirmation");
            var doctor = context.doctors.Include(d=>d.account).FirstOrDefault(d => d.id == idDoc);
            doctor.verified = true;
            context.SaveChanges();
            if (AccountController.EmailIsValid(doctor.account.email))
            {
                EmailService emailService = new EmailService();
                List<string> confirmLink = new List<string>();
                string message = await templateService.GetTemplateHtmlAsStringAsync("EmailTemplates/DoctorAccountConfirmationEmail", new List<string> { "DoctorAccountAcceptedMessage" });
                await emailService.SendEmailAsync(doctor.account.email, "Verification", message, "DoctorAplication");
            }
            else
            {
                return RedirectToAction("DoctorApplicationConfirmation");
            }
            //Logs
            context.logEvents.Add(LogEvent.createLog(
                   HttpContext.Connection.RemoteIpAddress?.ToString(),
                   "Doctor Application/id Doctor = " + idDoc + "/Accepted",
                   context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                   "Succes"
                   ));
            await context.SaveChangesAsync();
            return RedirectToAction("DoctorApplicationConfirmation", new { page = page });
        }

        public async Task<IActionResult> DenyDoctor(int idDoc, int page = 1)
        {
            if (context.doctors.Any(d => d.id == idDoc) is false || idDoc is 0) return RedirectToAction("DoctorApplicationConfirmation");
            var doctor = context.doctors.Include(d=>d.account).FirstOrDefault(d => d.id == idDoc);
            //Email
            doctor.specialities = new Collection<DoctorSpecialitie>();
            context.SaveChanges();
            context.Remove(doctor);
            context.SaveChanges();
            if (AccountController.EmailIsValid(doctor.account.email))
            {
                EmailService emailService = new EmailService();
                List<string> confirmLink = new List<string>();
                string message = await templateService.GetTemplateHtmlAsStringAsync("EmailTemplates/DoctorAccountConfirmationEmail", new List<string> { "DoctorAccountDeniedMessage" });
                await emailService.SendEmailAsync(doctor.account.email, "Verification", message, "DoctorAplication");
            }
            else
            {
                return RedirectToAction("DoctorApplicationConfirmation");
            }
            context.logEvents.Add(LogEvent.createLog(
                   HttpContext.Connection.RemoteIpAddress?.ToString(),
                   "Doctor Application/id Doctor = " + idDoc + "/Denied",
                   context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                   "Succes"
                   ));
            await context.SaveChangesAsync();

            return RedirectToAction("DoctorApplicationConfirmation", new { page = page });
        }


    }
}
