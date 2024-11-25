using System.ComponentModel.DataAnnotations;

namespace DoctorApplication.Models.DbEntities
{
    public class DoctorSpecialitie
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public bool enabled { get; set; }
        public virtual ICollection<Doctor> doctors { get; set; }
    }
}
