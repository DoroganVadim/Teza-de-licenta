using DoctorApplication.Models.DbEntities;

namespace DoctorApplication.Models.Doctor
{
    public class DoctorSpecialityModel
    {
        public DbEntities.Doctor doctor { get; set; }
        public IEnumerable<DoctorSpecialitie> doctorSpecialities { get; set; }
        public string password { get; set; }
        public string repPassword { get; set; }
    }
}
