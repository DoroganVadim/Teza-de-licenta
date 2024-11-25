using DoctorApplication.Models;
using DoctorApplication.Models.DbEntities;
using DoctorApplication.Models.Home;
using DoctorApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DoctorApplication.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly DoctorAppDbContext context;
        private readonly IEmailTemplateService _templateService;

        public HomeController(DoctorAppDbContext Context, IEmailTemplateService templateService)
        {
            _templateService = templateService;
            this.context = Context;
        }

        [HttpPost]
        public async Task<IActionResult> Index(HomeModel model)
        {
            TimeSpan timp = TimeSpan.Parse(model.time);
            Appointment inr = new Appointment()
            {
                namePacient = model.name,
                surrnamePacient = model.surrname,
                doctor = context.doctors.First(d=>d.id == model.doctor),
                tel = model.tel,
                appointmentDate = model.date + TimeSpan.Parse(model.time),
                appointmentTime = TimeSpan.Parse(model.time),
                confirmedUser = false,
                confirmedDoctor = false,
                emailUser = User.Identity.Name
            };
            context.Add(inr);
            context.SaveChanges();
            EmailService emailService = new EmailService();
            string confirmLink = Url.Action("AppointmentConfirmation", "Confirmation", new { idInreg = inr.id, email = User.Identity.Name }, Request.Scheme);
            string annulLink = Url.Action("AppointmentAnnul", "Confirmation", new { idInreg = inr.id, email = User.Identity.Name }, Request.Scheme);
            var docName = context.doctors.First(d => d.id == model.doctor);


            EmailComfirmationModel confirmationEmailModel = new EmailComfirmationModel { patientFullName = inr.surrnamePacient + " " + inr.namePacient, phone = inr.tel, doctorFullName = docName.surrname + " " + docName.name, day = inr.appointmentDate.ToString("dd-MM-yyyy"), time = inr.appointmentTime.ToString(), confirmLink = confirmLink, annulLink = annulLink };
            string message = await _templateService.GetTemplateHtmlAsStringAsync("EmailTemplates/AppointmentConfirmationEmail", confirmationEmailModel);
            await emailService.SendEmailAsync(User.Identity.Name, "Confirmation", message, "DoctorAplication");
            var newmodel = new HomeModel
            {
                doctors = context.doctors.Include(d => d.specialities).Where(d => d.id == 1).ToList()
            };
            context.logEvents.Add(LogEvent.createLog(
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "Appointment Confirmation Sent",
                    context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                    "Succes"
                ));
            context.SaveChanges();
            return View(newmodel);
        }

        public IActionResult doctorImageLoad(int docId)
        {
            string imageStr = context.doctors.Where(d => d.id == docId).FirstOrDefault().imageString;
            if (imageStr is null)
            {
                return File("", "image/png");
            }
            byte[] image = Convert.FromBase64String(imageStr);
            return File(image, "image/png");

        }

        public IActionResult Index()
        {
            var model = new HomeModel
            {
                doctors = context.doctors.Include(d => d.specialities).Include(d => d.account).Where(d => d.verified == true && d.activityStatus == true).OrderBy(d => d.surrname).ThenBy(d => d.name).ToList()
            };
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult GetTimes(DateTime date, int idDoc)
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


            var noTimes = context.appointments.Where(i => i.appointmentDate == date && i.doctor.id == idDoc && i.confirmedUser == true).Select(i => i.appointmentTime).ToList();
            List<string> finalTimes = new List<string>();
            foreach (TimeSpan t in noTimes)
            {
                string str = "";
                if (t.Hours < 10) str += "0";
                str += Convert.ToString(t.Hours);
                str += ":";
                if (t.Minutes < 10) str += "0";
                str += Convert.ToString(t.Minutes);
                finalTimes.Add(str);
            }
            return Json(baseTimes.Except(finalTimes));
        }

        public string ConformationUrl()
        {
            return Url.Action("ConfirmationSent", "Home");
        }

        public IActionResult Back()
        {
            return RedirectToAction("Index");
        }

        public IActionResult ConfirmationSent()
        {
            return View();
        }

        public IActionResult ChageModeToAdmin()
        {
            return RedirectToAction("Index", "Doctor");
        }

        public IActionResult DoctorProfile(int idDoc)
        {
            if (context.doctors.Any(d => d.id == idDoc) == false) return RedirectToAction("Index", "Home");
            return View(context.doctors.Include(d => d.specialities).Include(d=>d.account).Where(d => d.id == idDoc).First());
        }

        public IActionResult DoctorAccountProfile()
        {
            DoctorAccountProfileModel accountProfile = new DoctorAccountProfileModel
            {
                doctor = context.doctors.Include(d => d.specialities).Include(d => d.account).FirstOrDefault(d => d.account.email == User.Identity.Name),
            };
            if (accountProfile.doctor == null) return RedirectToAction("Index");
            return View(accountProfile);
        }

        public async Task<IActionResult> DoctorAccountAppointments(int tab = 1, int page = 1, int pageToBeAccepted = 1)
        {
            if (page < 1) page = 1;
            if (pageToBeAccepted < 1) pageToBeAccepted = 1;
            if (tab == 1) TempData["Tab"] = 1;
            if (tab == 2) TempData["Tab"] = 2;
            DoctorAccountProfileModel accountProfile = new DoctorAccountProfileModel
            {
                doctor = context.doctors.Include(d => d.specialities).FirstOrDefault(d => d.account.email == User.Identity.Name),
            };
            if (accountProfile.doctor == null) return RedirectToAction("Index");
            int pageSize = 5;

            var appointmentsToBeAccepted = context.appointments.Where(i => i.doctor.id == accountProfile.doctor.id && i.confirmedDoctor == false && i.confirmedUser == true && i.appointmentDate.CompareTo(DateTime.Now.Date) >= 0).OrderBy(ap => ap.appointmentDate);
            var appointments = context.appointments.Where(i => i.doctor.id == accountProfile.doctor.id && i.confirmedDoctor == true && i.confirmedUser == true && i.appointmentDate.CompareTo(DateTime.Now.Date) >= 0).OrderBy(ap => ap.appointmentDate);

            int countToBeAccepted = 0;
            int count = 0;
            try
            {
                countToBeAccepted = await appointmentsToBeAccepted.CountAsync();
                count = await appointments.CountAsync();
            }
            catch(Exception e)
            {
                countToBeAccepted = 0;
                count = 0;
            }
            try
            {
                accountProfile.appointmentsToBeAccepted = await appointmentsToBeAccepted.Skip((pageToBeAccepted - 1) * pageSize).Take(pageSize).ToListAsync();
                accountProfile.appointments = await appointments.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                accountProfile.page = new PageViewModel(count, page, pageSize);
                accountProfile.pageToBeAccepted = new PageViewModel(countToBeAccepted, pageToBeAccepted, pageSize);
            }
            catch(Exception e)
            {
                accountProfile.appointmentsToBeAccepted = new List<Appointment>();
                accountProfile.appointments = new List<Appointment>();
                accountProfile.page = new PageViewModel(0, 0, 0);
                accountProfile.pageToBeAccepted = new PageViewModel(0, 0, 0);
            }
            return View(accountProfile);
        }
        public IActionResult DenyAppointment(int id, int page = 1, int pageToBeAccepted = 1, int tab = 1)
        {
            if (id <= 0)
            {
                return RedirectToAction("DoctorAccountAppointments");
            }
            var appointment = context.appointments.First(a => a.id == id);
            appointment.confirmedDoctor = false;
            context.logEvents.Add(LogEvent.createLog(
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                "Denied Appointment/Id = " + id,
                context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                "Succes"
                ));
            context.SaveChanges();
            return RedirectToAction("DoctorAccountAppointments", new { tab = tab, page = page, pageToBeAccepted = pageToBeAccepted });
        }

        public IActionResult AcceptAppointment(int id, int page = 1, int pageToBeAccepted = 1, int tab = 1)
        {
            if (id <= 0)
            {
                return RedirectToAction("DoctorAccountAppointments");
            }
            var appointment = context.appointments.First(a => a.id == id);
            appointment.confirmedDoctor = true;
            context.logEvents.Add(LogEvent.createLog(
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                "Accepted Appointment/Id = " + id,
                context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                "Succes"
                ));
            context.SaveChanges();
            return RedirectToAction("DoctorAccountAppointments", new { tab = tab, page = page, pageToBeAccepted = pageToBeAccepted });
        }

        public async Task<IActionResult> MyAppointments(int page = 1)
        {
            if (page < 1) page = 1;
            int pageSize = 4;
            var appointments = context.appointments.Where(i => i.emailUser == User.Identity.Name && i.confirmedUser == true && i.appointmentDate.CompareTo(DateTime.Now.Date) >= 0).OrderBy(ap => ap.appointmentDate);
            var count = await appointments.CountAsync();
            UserAppointmentsViewModel model = new UserAppointmentsViewModel();
            try
            {
                model.appointments = await appointments.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                model.doctors = await context.doctors.ToListAsync();
                model.pageViewModel = new PageViewModel(count, page, pageSize);
            }catch (Exception e)
            {
                model.appointments = new List<Appointment>();
                model.doctors = new List<Doctor>();
                model.pageViewModel = new PageViewModel(count, page, pageSize);
            }
            return View(model);
        }

        public IActionResult AnnulAppointment(int id, int page = 1)
        {
            if (page < 1) page = 1;
            if (id < 1) return RedirectToAction("MyAppointments", "Home");
            var appointment = context.appointments.First(a => a.id == id);
            context.Remove(appointment);
            context.SaveChanges();
            return RedirectToAction("MyAppointments", "Home", new { page = page });
        }
    }
}