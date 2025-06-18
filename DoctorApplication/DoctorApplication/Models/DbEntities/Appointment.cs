using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorApplication.Models.DbEntities
{
    public class Appointment
    {
        [Key]
        public int id { get; set; }
        public string namePacient { get; set; }
        public string surrnamePacient { get; set; }
        public int doctorId { get; set; }
        [ForeignKey("doctorId")]
        public virtual Doctor doctor { get; set; }
        public DateTime appointmentDate { get; set; }
        public TimeSpan appointmentTime { get; set; }
        public string tel { get; set; }
        public bool confirmedUser { get; set; }
        public DateTime created { get; set; } = DateTime.Now;
        public bool confirmedDoctor { get; set; }
        public string emailUser { get; set; }
    }
}
