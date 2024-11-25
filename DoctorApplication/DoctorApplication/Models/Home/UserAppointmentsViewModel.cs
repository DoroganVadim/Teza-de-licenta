using DoctorApplication.Models.DbEntities;

namespace DoctorApplication.Models.Home
{
    public class UserAppointmentsViewModel
    {
        public IEnumerable<DbEntities.Doctor> doctors { get; set; }
        public IEnumerable<Appointment> appointments { get; set; }
        public PageViewModel pageViewModel { get; set; }
    }
}
