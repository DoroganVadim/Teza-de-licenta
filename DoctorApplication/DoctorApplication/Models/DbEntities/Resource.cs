using System.ComponentModel.DataAnnotations;

namespace DoctorApplication.Models.DbEntities
{
    public class Resource
    {
        [Key]
        public int id { get; set; }
        public string key { get; set; }
        public string value { get; set; }
        public virtual Culture culture { get; set; }
    }
}
