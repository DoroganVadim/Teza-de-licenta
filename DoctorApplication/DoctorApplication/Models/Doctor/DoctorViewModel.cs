using DoctorApplication.Models.DbEntities;
using DoctorApplication.Models.Home;

namespace DoctorApplication.Models.Doctor
{
    public class DoctorViewModel
    {
        public IEnumerable<DbEntities.Doctor> doctors { get; set; }
        public IEnumerable<DoctorSpecialitie> doctorTypes { get; set; }
        public PageViewModel pageViewModel { get; set; }
    }
}
