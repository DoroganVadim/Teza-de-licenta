namespace DoctorApplication.Models.Statistics
{
    public class StatisticsModel
    {
        public List<int> eachRoleNum { get; set; }
        public List<int> activityCount { get; set; }
        public IEnumerable<string> doctorsNames { get; set; }
        public IEnumerable<int> doctorsAppointmentNum { get; set; }
    }
}
