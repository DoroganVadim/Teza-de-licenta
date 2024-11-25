using System.ComponentModel.DataAnnotations;

namespace DoctorApplication.Models.DbEntities
{
    public class LogEvent
    {
        [Key]
        public int id { get; set; }
        public string ip { get; set; }
        public DateTime date { get; set; }
        public string _event { get; set; }
        public int userId { get; set; }
        public virtual Account user { get; set; }
        public string type { get; set; }

        public static LogEvent createLog(string ip, string _event, Account user, string type)
        {
            return new LogEvent { ip = ip, date = DateTime.Now, _event = _event, user = user, type = type };
        }
        public static LogEvent createLog(string ip, string _event, string type)
        {
            return new LogEvent { ip = ip, date = DateTime.Now, _event = _event, type = type };
        }
    }
}
