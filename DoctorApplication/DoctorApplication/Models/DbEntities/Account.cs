
using System.ComponentModel.DataAnnotations;

namespace DoctorApplication.Models.DbEntities
{
    public class Account
    {
        [Key]
        public int id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int role { get; set; }
        public bool verified { get; set; }
        public string? token { get; set; }
    }
}
