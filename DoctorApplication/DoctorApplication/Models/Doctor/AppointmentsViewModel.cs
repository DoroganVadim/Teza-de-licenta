using DoctorApplication.Models.DbEntities;
using DoctorApplication.Models.Home;

namespace DoctorApplication.Models.Doctor
{
    public class AppointmentsViewModel
    {
        public IEnumerable<Appointment> appointments { get; set; }
        public PageViewModel pageViewModel { get; set; }
        public int idDoctor { get; set; }
    }
}
