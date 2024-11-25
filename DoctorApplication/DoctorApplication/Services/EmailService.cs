using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace DoctorApplication.Services
{
    public class EmailService
    {
        public async Task SendEmailAsync(string emailAdressTo, string subject, string message, string fromName)
        {
            string login = "doroganvadimpracticatest@gmail.com";
            string password = "rhcwdnnkucqwladz";
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(fromName, login));
            emailMessage.To.Add(new MailboxAddress("", emailAdressTo));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("Html") { Text = message };


            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(login, password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
