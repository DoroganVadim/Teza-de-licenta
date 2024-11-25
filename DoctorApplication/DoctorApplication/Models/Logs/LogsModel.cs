using DoctorApplication.Models.DbEntities;
using DoctorApplication.Models.Home;

namespace DoctorApplication.Models.Logs
{
    public class LogsModel
    {
        public IEnumerable<LogEvent> logs { get; set; }
        public PageViewModel pageViewModel { get; set; }
    }
}
