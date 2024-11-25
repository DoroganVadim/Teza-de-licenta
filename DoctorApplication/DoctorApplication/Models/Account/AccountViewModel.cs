using DoctorApplication.Models.Account;
using DoctorApplication.Models.DbEntities;

namespace DoctorApplication.Models.Account
{
    public class AccountViewModel
    {
        public LoginModel loginModel { get; set; }
        public RegisterModel registerModel { get; set; }
        public DoctorApplication.Models.DbEntities.Doctor doctorAccount { get; set; }

        public AccountViewModel()
        {
            loginModel = new LoginModel
            {
                email = "",
                password = ""
            };

            registerModel = new RegisterModel
            {
                email = "",
                password = "",
                confirmPassword = ""
            };
        }
    }
}
