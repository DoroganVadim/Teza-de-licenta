using DoctorApplication.Models.DbEntities;

namespace DoctorApplication.Models.Account
{
    public class DoctorSpecialityModel
    {
        public DbEntities.Doctor doctor { get; set; }
        public IEnumerable<DoctorSpecialitie> doctorSpeciality { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
    }
}
