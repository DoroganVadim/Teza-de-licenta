using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace DoctorApplication.Models.DbEntities
{
    public class Doctor
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string surrname { get; set; }
        public string tel { get; set; }
        //public string email { get; set; }
        public virtual Account? account { get; set; }
        public bool activityStatus { get; set; }
        public int appointmentNum { get; set; }
        public string? imageString { get; set; }
        public virtual ICollection<DoctorSpecialitie> specialities { get; set; }
        public bool verified { get; set; }

        public Doctor()
        {
            specialities = new Collection<DoctorSpecialitie>();
        }
    }
}
