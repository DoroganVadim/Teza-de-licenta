using DoctorApplication.Models;
using DoctorApplication.Models.DbEntities;
using DoctorApplication.Models.EmailTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorApplication.Controllers
{
    [Authorize(Roles = "admin")]
    public class EmailTemplatesController : Controller
    {
        private DoctorAppDbContext context;
        public EmailTemplatesController(DoctorAppDbContext context)
        {
            this.context = context;
        }
        public IActionResult AppointmentConfirmationEmail()
        {

            ConfirmationEmailModel cem = new ConfirmationEmailModel();
            return View(cem);
        }

        public IActionResult AccountConfirmationEmail()
        {
            List<string> str = new List<string>();
            str.Add("word");
            return View("AccountConfirmationEmail", str);
        }

        public IActionResult PasswordResetEmail()
        {
            List<string> str = new List<string>();
            str.Add("word");
            return View("PasswordResetEmail", str);
        }
        public IActionResult DoctorAccountConfirmationEmail()
        {
            List<string> str = new List<string>();
            str.Add("DoctorAccountDeniedMessage");
            return View(str);
        }

        public IActionResult DoctorAccountApplicationAnnul(Doctor d = null)
        {

            return View(context.doctors.First());
        }
    }
}
