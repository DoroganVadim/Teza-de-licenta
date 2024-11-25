using DoctorApplication.Classes;
using DoctorApplication.Models.DbEntities;
using DoctorApplication.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Text;
using DoctorApplication.Models.Account;
using DoctorApplication.Models;

namespace DoctorApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly DoctorAppDbContext context;
        private readonly IEmailTemplateService templateService;
        public AccountController(DoctorAppDbContext context, IEmailTemplateService templateService)
        {
            this.templateService = templateService;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public static string CreateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountViewModel accountModel)
        {
            var aux = context.logEvents.OrderByDescending(i => i.id).FirstOrDefault();
            LoginModel model = accountModel.loginModel;
            model.password = CreateMD5(model.password);
            if (context.accounts.Any(u => u.email == model.email && u.password == model.password))
            {
                if (context.accounts.First(u => u.email == model.email).verified == false)
                {
                    TempData["LogInMessage"] = "EmailNotConfirmed";
                    return View();
                }
                await Authenticate(model.email, context.accounts.FirstOrDefault(u => u.email == model.email && u.password == model.password).role, context.doctors.Any(d => d.account.email == model.email && d.activityStatus == true && d.verified == true));
                var user = context.accounts.First(u => u.email == model.email);
                context.logEvents.Add(LogEvent.createLog(
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "Login",
                    user,
                    "Succes"
                    ));
                context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["LogInMessage"] = "IncorrectLogInOrPassword";
                var user = context.accounts.First(u => u.email == model.email);
                context.logEvents.Add(LogEvent.createLog(
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "Login",
                    user,
                    "Error"
                    ));
                context.SaveChanges();
                return View();
            }
        }

        public static bool EmailIsValid(string emailaddress)
        {
            if (emailaddress is null) return false;
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AccountViewModel accountModel)
        {
            RegisterModel model = accountModel.registerModel;
            if (context.accounts.Any(u => u.email == model.email))
            {
                TempData["RegisterMessage"] = "IncorrectEmail";
                TempData["RegisterEmailMessage"] = "AccountAlreadyExists";
                return RedirectToAction("Login", "Account", accountModel);
            }
            bool isValid = true;
            TempData["RegisterEmail"] = model.email;
            if (EmailIsValid(model.email) is false)
            {
                isValid = false;
                TempData["RegisterEmailMessage"] = "IncorrectEmail";
            }

            Regex regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,16}$");
            if (model.password is null || regex.IsMatch(model.password) is false)
            {
                isValid = false;
                TempData["RegisterPasswordMessage"] = "PasswordTooWeak";
            }

            if (model.confirmPassword != model.password)
            {
                TempData["RegisterPassword"] = model.password;
                isValid = false;
                TempData["RegisterRepeatPasswordMessage"] = "DoesNotMatch";
            }

            if (isValid is false)
            {
                TempData["RegisterMessage"] = "IncorrectEmail";
                return RedirectToAction("Login", "Account", new AccountViewModel() { registerModel = model });
            }

            TempData["RegisterEmail"] = "";
            string token = Guid.NewGuid().ToString();
            EmailService emailService = new EmailService();
            try
            {
                List<string> confirmLink = new List<string>();
                confirmLink.Add(Url.Action("EmailConfirmation", "Confirmation", new { emailToBeVerified = model.email, token = token }, Request.Scheme));
                string message = await this.templateService.GetTemplateHtmlAsStringAsync("EmailTemplates/AccountConfirmationEmail", confirmLink);
                await emailService.SendEmailAsync(model.email, "Verification", message, "DoctorAplication");
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
            int role = 0;
            if (context.accounts.Count() == 0) role = 1;
            context.accounts.Add(new Account
            {
                email = model.email,
                password = CreateMD5(model.password),
                role = role,
                verified = false,
                token = token
            });
            await context.SaveChangesAsync();
            context.logEvents.Add(LogEvent.createLog(
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "Register",
                    context.accounts.Where(u => u.email == model.email).FirstOrDefault(),
                    "Succes"
                    ));
            await context.SaveChangesAsync();
            TempData["emailMessage"] = "MesageSentToYourEmail";
            return RedirectToAction("MessageSent", "Account");
        }

        public IActionResult ResetPassword()
        {
            return View();
        }
        public IActionResult MessageSent(string s)
        {
            return View(s);
        }

        public async Task<IActionResult> SendPasswordReset(string eMail)
        {
            if (eMail is null)
            {
                TempData["PasswordResetMessage"] = "EnterEmailAdress";
                TempData["PasswordResetTypeMessage"] = "alert-warning";
                return RedirectToAction("ResetPassword");
            }
            var user = context.accounts.Where(u => u.email == eMail).FirstOrDefault();
            if (user != null)
            {
                if (user.verified == false)
                {
                    TempData["PasswordResetMessage"] = "EmailNotConfirmed";
                    TempData["PasswordResetTypeMessage"] = "alert-danger";
                    return RedirectToAction("ResetPassword");
                }
                string token = Guid.NewGuid().ToString();
                EmailService emailService = new EmailService();
                List<string> confirmLink = new List<string>();
                confirmLink.Add(Url.Action("PasswordReset", "Confirmation", new { emailToBeVerified = eMail, token = token }, Request.Scheme));
                string message = await templateService.GetTemplateHtmlAsStringAsync("EmailTemplates/PasswordResetEmail", confirmLink);
                await emailService.SendEmailAsync(eMail, "Verification", message, "DoctorAplication");
                user.token = token;
                context.SaveChanges();
                TempData["emailMessage"] = "MesageSentToYourEmail";
                return RedirectToAction("MessageSent", "Account");
            }
            else
            {
                TempData["PasswordResetMessage"] = "EmailNonExistent";
                TempData["PasswordResetTypeMessage"] = "alert-danger";
                return RedirectToAction("ResetPassword");
            }
        }

        private async Task Authenticate(string userName, int role, bool isDoctor)
        {
            string strRole = "";
            if (role == 0) strRole = "user";
            if (role == 1) strRole = "admin";


            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType,userName),
                    new Claim(ClaimTypes.Role,strRole)
                };
            if (isDoctor is true)
            {
                claims.Add(new Claim("isDoctor", "true"));
            }
            else
            {
                claims.Add(new Claim("isDoctor", "false"));
            }

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            context.logEvents.Add(LogEvent.createLog(
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "Logout",
                    context.accounts.Where(u => u.email == User.Identity.Name).FirstOrDefault(),
                    "Succes"
                    ));
            context.SaveChanges();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult AccesDenied()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        public IActionResult Back()
        {
            return RedirectToAction("Login");
        }

        public IActionResult DoctorAccountApplication(DoctorSpecialityModel model)
        {
            var types = context.doctorSpecialities.Where(d => d.enabled == true);
            return View(new DoctorSpecialityModel { doctor = new Doctor(), doctorSpeciality = types });
        }

        [HttpPost]
        public async Task<IActionResult> RegisterDoctorUser(DoctorSpecialityModel submitted, int[] ids, IFormFile file, bool hasAccount)
        {
            RegisterModel model = new RegisterModel
            {
                email = submitted.doctor.account.email,
                password = submitted.password,
                confirmPassword = submitted.confirmPassword
            };
            bool isValid = true;

            TempData["RegisterEmail"] = model.email;
            if (EmailIsValid(model.email) is false)
            {
                isValid = false;
                TempData["RegisterEmailMessage"] = "IncorrectEmail";
            }


            if (hasAccount is false)
            {
                if (context.accounts.Any(u => u.email == model.email))
                {
                    isValid = false;
                    TempData["RegisterMessage"] = "IncorrectEmail";
                    TempData["RegisterEmailMessage"] = "AccountAlreadyExists";
                }
                Regex regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");
                if (model.password is null || regex.IsMatch(model.password) is false)
                {
                    isValid = false;
                    TempData["RegisterPasswordMessage"] = "PasswordTooWeak";
                }

                if (model.confirmPassword != model.password)
                {
                    TempData["RegisterPassword"] = model.password;
                    isValid = false;
                    TempData["RegisterRepeatPasswordMessage"] = "DoesNotMatch";
                }
            }
            else
            {
                if (context.accounts.Any(u => u.email == model.email) == false)
                {
                    isValid = false;
                    TempData["RegisterMessage"] = "IncorrectEmail";
                    TempData["RegisterEmailMessage"] = "IncorrectEmail";
                }
            }

            if (context.doctors.Any(d => d.account.email == model.email))
            {
                TempData["RegisterMessage"] = "IncorrectEmail";
                TempData["RegisterEmailMessage"] = "DoctorAccountAlreadyExists";
            }

            if (isValid is false)
            {
                TempData["RegisterMessage"] = "IncorrectEmail";
                if (hasAccount == true) TempData["Selected"] = "Have";
                return RedirectToAction("DoctorAccountApplication");
            }

            Account u = new Account();
            if (hasAccount is false)
            {
                string token = Guid.NewGuid().ToString();
                EmailService emailService = new EmailService();
                List<string> confirmLink = new List<string>();
                confirmLink.Add(Url.Action("EmailConfirmation", "Confirmation", new { emailToBeVerified = model.email, token = token }, Request.Scheme));
                string message = await templateService.GetTemplateHtmlAsStringAsync("EmailTemplates/AccountConfirmationEmail", confirmLink);
                await emailService.SendEmailAsync(model.email, "Verification", message, "DoctorAplication");
                u = new Account();
                u.email = submitted.doctor.account.email;
                u.password = CreateMD5(submitted.password);
                u.role = 0;
                u.token = token;
                u.verified = false;
                if (context.accounts.Count() == 0) u.role = 1;
                context.accounts.Add(u);
                context.SaveChanges();
            }

            Doctor d = new Doctor();
            d.surrname = submitted.doctor.surrname;
            d.name = submitted.doctor.name;
            d.account = u;
            d.tel = submitted.doctor.tel;
            d.activityStatus = true;
            d.imageString = ImageStringConvert.ConvertImageToString(file);
            foreach (var id in ids)
            {
                var type = context.doctorSpecialities.First(d => d.id == id);
                d.specialities.Add(type);
            }
            context.doctors.Add(d);
            await context.SaveChangesAsync();
            context.logEvents.Add(LogEvent.createLog(
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "Doctor Register",
                    context.accounts.Where(u => u.email == model.email).FirstOrDefault(),
                    "Succes"
                    ));
            await context.SaveChangesAsync();
            if (hasAccount is false)
            {
                TempData["emailMessage"] = "MesageSentToYourEmailForDoctors";
                return RedirectToAction("MessageSent", "Account");
            }
            else
            {
                TempData["emailMessage"] = "YourDoctorAccountSubmittionWasRecieved";
                return RedirectToAction("MessageSent", "Account");
            }
        }
    }
}
