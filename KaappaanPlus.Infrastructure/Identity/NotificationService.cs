using KaappaanPlus.Application.Contracts.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Infrastructure.Identity
{
    public class NotificationService : INotificationService
    {
        private readonly IConfiguration _config;

        public NotificationService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpHost = _config["EmailSettings:SmtpHost"]!;
            var smtpUser = _config["EmailSettings:SmtpUser"]!;
            var smtpPass = _config["EmailSettings:SmtpPassword"]!;
            var smtpPort = int.Parse(_config["EmailSettings:SmtpPort"]!);


            var mail = new MailMessage
            {
                From = new MailAddress(smtpUser, "Kaappaan System"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mail.To.Add(to);

            using var smtp = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mail);
        }
    }
}
