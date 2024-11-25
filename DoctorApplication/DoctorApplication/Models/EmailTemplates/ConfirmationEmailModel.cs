namespace DoctorApplication.Models.EmailTemplates
{
    public class ConfirmationEmailModel
    {
        public string patientFullName { get; set; }
        public string tel { get; set; }
        public string doctorFullName { get; set; }
        public string day { get; set; }
        public string time { get; set; }
        public string confirmLink { get; set; }
        public string annulLink { get; set; }
    }
}
