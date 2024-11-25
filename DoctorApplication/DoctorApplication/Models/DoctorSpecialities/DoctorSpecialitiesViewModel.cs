using DoctorApplication.Models.DbEntities;
using DoctorApplication.Models.Home;

namespace DoctorApplication.Models.DoctorSpecialities
{
    public class DoctorSpecialitiesViewModel
    {
        public List<DoctorSpecialitie> docSpecs { get; set; }
        public PageViewModel pageViewModel { get; set; }
    }
}
