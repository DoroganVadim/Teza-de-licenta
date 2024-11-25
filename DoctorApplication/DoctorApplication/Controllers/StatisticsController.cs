using DoctorApplication.Models;
using DoctorApplication.Models.Statistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorApplication.Controllers
{
    [Authorize(Roles = "admin")]
    public class StatisticsController : Controller
    {
        private DoctorAppDbContext context;
        public StatisticsController(DoctorAppDbContext Context)
        {
            this.context = Context;
        }
        public IActionResult Index()
        {
            List<int> eachRoleNum = new List<int>();
            eachRoleNum.Add(context.accounts.Where(u => u.role == 0).Count());
            eachRoleNum.Add(context.accounts.Where(u => u.role == 1).Count());

            List<int> activitycount = new List<int>();
            activitycount.Add(context.doctors.Where(d => d.activityStatus == true).Count());
            activitycount.Add(context.doctors.Where(d => d.activityStatus == false).Count());

            var doctorsNames = context.doctors.Where(d => d.activityStatus == true).Select(d => d.surrname + " " + d.name);
            var doctorsAppointmentNum = context.doctors.Where(d => d.activityStatus == true).Select(d => d.appointmentNum);
            StatisticsModel sw = new StatisticsModel { eachRoleNum = eachRoleNum, activityCount = activitycount, doctorsNames = (IEnumerable<string>)doctorsNames, doctorsAppointmentNum = (IEnumerable<int>)doctorsAppointmentNum };
            return View(sw);
        }
    }
}
