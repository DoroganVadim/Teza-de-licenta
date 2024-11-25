using DoctorApplication.Models.DbEntities;

namespace DoctorApplication.Models.Home
{
    public class DoctorAccountProfileModel
    {
        public DbEntities.Doctor doctor { get; set; }
        public List<Appointment> appointmentsToBeAccepted { get; set; }
        public List<Appointment> appointments { get; set; }
        public PageViewModel pageToBeAccepted { get; set; }
        public PageViewModel page { get; set; }
    }
}
