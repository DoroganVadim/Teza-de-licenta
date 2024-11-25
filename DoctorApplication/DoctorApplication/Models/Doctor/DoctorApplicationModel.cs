using DoctorApplication.Models.Home;

namespace DoctorApplication.Models.Doctor
{
    public class DoctorApplicationModel
    {
        public IEnumerable<DbEntities.Doctor> doctors { get; set; }
        public PageViewModel pageViewModel { get; set; }
    }
}
