using ClothesShopApp.Data.Entity;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Net.Mail;

namespace ClothesShopApp.Areas.Account.Models
{
    public class EmailSender : IEmailSender<User> 
    {
        private readonly SmtpClient _smtpClient;

        public EmailSender()
        {
            _smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("tamerkndl.03@gmail.com", "pbte wtiv eoph begu"),
                EnableSsl = true
            };
        }
        public Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
        {
            throw new NotImplementedException();
        }

        public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
        {
            throw new NotImplementedException();
        }

        public async Task SendPasswordResetLinkAsync(User user,string email, string? resetLink)
        {
            var mailMessage = new MailMessage("tamerkndl.03@gmail.com", email)
            {
                Subject = "Şifrenizi Sıfırlayın.",
                Body = $"Şifrenizi sıfırlamak için <a href='{resetLink}'>buraya tıkalyn</a>.",
                IsBodyHtml = true
            };
            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
