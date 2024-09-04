using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;

namespace AspNetIdentityDemo.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SmtpClient client = new SmtpClient("localhost", 25);
            //client.UseDefaultCredentials = true;
            //client.Credentials = new NetworkCredential("username", "password");
            MailMessage mailMessage = new MailMessage("security@company.com", email
                , subject, htmlMessage);
            mailMessage.IsBodyHtml = true;
            await client.SendMailAsync(mailMessage);
        }
    }
}
