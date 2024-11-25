using DoctorApplication.Models.DbEntities;

namespace DoctorApplication.Models.Home
{
    public class HomeModel
    {
        public string name { get; set; }
        public string surrname { get; set; }
        public int doctor { get; set; }
        public string tel { get; set; }
        public DateTime date { get; set; }
        public string time { get; set; }
        public IEnumerable<DbEntities.Doctor> doctors { get; set; }
    }
}
