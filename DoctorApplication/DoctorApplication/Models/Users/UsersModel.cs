using DoctorApplication.Models.Home;

namespace DoctorApplication.Models.Users
{
    public class UsersModel
    {
        public IEnumerable<DbEntities.Account> users { get; set; }
        public PageViewModel pageViewModel { get; set; }
    }
}
