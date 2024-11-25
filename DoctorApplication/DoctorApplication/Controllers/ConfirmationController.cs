using DoctorApplication.Models;
using DoctorApplication.Models.Confirmation;
using DoctorApplication.Models.DbEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace DoctorApplication.Controllers
{
    public class ConfirmationController : Controller
    {
        private  DoctorAppDbContext context;
        public ConfirmationController(DoctorAppDbContext context)
        {
            this.context = context;
        }
        public IActionResult EmailConfirmation(string emailToBeVerified, string token)
        {
            if (emailToBeVerified is null || token == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (context.accounts.Any(u => u.email == emailToBeVerified) is false)
            {
                return RedirectToAction("Login", "Account");
            }
            if (context.accounts.Where(u => u.email == emailToBeVerified).FirstOrDefault().verified == true)
            {
                TempData["ConfirmationAction"] = "EmailWasVerifiedAlready";
                return RedirectToAction("Index");
            }
            var verification = context.accounts.FirstOrDefault(u => u.email == emailToBeVerified);
            if (verification.token != token)
            {
                context.logEvents.Add(LogEvent.createLog(
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "Email Confirmation",
                    context.accounts.Where(u => u.email == emailToBeVerified).FirstOrDefault(),
                    "Error"
                    ));
                context.SaveChanges();
                TempData["ConfirmationAction"] = "VerificationMessageIsOld";
                return RedirectToAction("Index");
            }
            verification.verified = true;
            context.logEvents.Add(LogEvent.createLog(
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "Email Confirmation",
                    context.accounts.Where(u => u.email == emailToBeVerified).FirstOrDefault(),
                    "Succes"
                    ));
            context.SaveChanges();
            TempData["ConfirmationAction"] = "EmailVerifiedText";
            return RedirectToAction("Index");
        }


        public IActionResult AppointmentConfirmation(int idAppoint, string email)
        {
            if (idAppoint is 0 || email is null || context.accounts.Any(u => u.email == email) == false)
            {
                return RedirectToAction("Login", "Account");
            }
            if (context.appointments.FirstOrDefault(a => a.id == idAppoint) is null)
            {
                return RedirectToAction("Index", "Home");
            }
            var appoint = context.appointments.First(a => a.id == idAppoint);
            if ((DateTime.Now - appoint.created).TotalHours > 1 || context.appointments.Any(a => a.id != appoint.id && a.appointmentDate == appoint.appointmentDate && a.appointmentTime == appoint.appointmentTime && a.doctor.id == appoint.doctor.id && a.confirmedUser == true))
            {
                context.logEvents.Add(LogEvent.createLog(
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                "Appointment Confirmation/id = " + idAppoint,
                context.accounts.Where(u => u.email == email).FirstOrDefault(),
                "Error"
            ));
                context.SaveChanges();
                context.appointments.Remove(appoint);
                context.SaveChanges();
                TempData["ConfirmationAction"] = "AppointmentOutdated";
                return RedirectToAction("Index");
            }
            Doctor doc = context.doctors.Where(d => d.id == appoint.doctor.id).FirstOrDefault();
            if (doc is null)
            {
                return RedirectToAction("Index", "Home");
            }
            doc.appointmentNum++;
            appoint.confirmedUser = true;
            context.logEvents.Add(LogEvent.createLog(
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                "Appointment Confirmation/id = " + idAppoint,
                context.accounts.Where(u => u.email == email).FirstOrDefault(),
                "Succes"
            ));
            context.SaveChanges();
            TempData["ConfirmationAction"] = "AppointmentVerifiedText";
            return RedirectToAction("Index");
        }
        public IActionResult AppointmentAnnul(int idInreg, string email)
        {
            if (idInreg is 0 || email is null || context.accounts.Any(u => u.email == email) == false)
            {
                return RedirectToAction("Login", "Account");
            }
            if (context.appointments.FirstOrDefault(a => a.id == idInreg) is null)
            {
                return RedirectToAction("Index", "Home");
            }
            var inreg = context.appointments.First(a => a.id == idInreg);
            if ((DateTime.Now - inreg.created).TotalHours > 1 || context.appointments.Include(a=>a.doctor).Any(a => a.id != inreg.id && a.appointmentDate == inreg.appointmentDate && a.appointmentTime == inreg.appointmentTime && a.doctor.id == inreg.doctor.id && a.confirmedUser == true))
            {
                context.logEvents.Add(LogEvent.createLog(
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                "Appointment Annul/id = " + idInreg,
                context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                "Error"
            ));
                context.appointments.Remove(inreg);
                context.SaveChanges();
                TempData["ConfirmationAction"] = "AppointmentOutdated";
                return RedirectToAction("Index");
            }
            context.appointments.Remove(inreg);
            context.logEvents.Add(LogEvent.createLog(
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                "Appointment Annul/id = " + idInreg,
                context.accounts.Where(u => u.email == email).FirstOrDefault(),
                "Succes"
            ));
            context.SaveChanges();
            TempData["ConfirmationAction"] = "AppointmentAnnulledText";
            return RedirectToAction("Index");
        }

        public IActionResult PasswordReset(string emailToBeVerified, string token)
        {
            if (emailToBeVerified is null || token == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var verification = context.accounts.First(u => u.email == emailToBeVerified);
            if (verification is null)
            {
                context.logEvents.Add(LogEvent.createLog(
                   HttpContext.Connection.RemoteIpAddress?.ToString(),
                   "Reset Password",
                   context.accounts.Where(u => u.email == emailToBeVerified).FirstOrDefault(),
                   "Error"
               ));
                context.SaveChanges();
                return RedirectToAction("Login", "Account");
            }
            if (verification.token != token)
            {
                TempData["ConfirmationAction"] = "PasswordResetMessageOutdated";
                return RedirectToAction("Index");
            }
            PasswordResetViewModel model = new PasswordResetViewModel()
            {
                email = emailToBeVerified,
                token = token
            };

            return View(model);
        }

        public IActionResult SendNewPassword(PasswordResetViewModel model, string password, string repPassword)
        {
            Regex regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");
            if (regex.IsMatch(password) is false)
            {
                TempData["PasswordResetMessage"] = "PasswordTooWeak";
                TempData["PasswordResetTypeMessage"] = "alert-danger";
                return RedirectToAction("PasswordReset", new { emailToBeVerified = model.email, token = model.token });
            }
            if (password != repPassword)
            {
                TempData["PasswordResetMessage"] = "DoesNotMatch";
                TempData["PasswordResetTypeMessage"] = "alert-danger";
                return RedirectToAction("PasswordReset", new { emailToBeVerified = model.email, token = model.token });
            }
            if (model.email is null || model.token == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var verification = context.accounts.First(u => u.email == model.email);
            if (verification is null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (verification.token != model.token)
            {
                TempData["ConfirmationAction"] = "ResetMessageOutdated";
                return RedirectToAction("Index");
            }
            verification.password = password;
            TempData["ConfirmationAction"] = "PasswordWasReseted";
            context.logEvents.Add(LogEvent.createLog(
                   HttpContext.Connection.RemoteIpAddress?.ToString(),
                   "Reset Password",
                   context.accounts.Where(u => u.email == model.email).FirstOrDefault(),
                   "Succes"
               ));
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
