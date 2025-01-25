using System.Net.Mail;
using System.Net;

namespace Registeration.Main.Application.Services
{
    public class EmailService(IConfiguration configuration) : IEmailService
    {
        private readonly string _smtpHost = configuration["Smtp:Host"] ?? "";
        private readonly int _smtpPort = int.Parse(configuration["Smtp:Port"] ?? "0");
        private readonly string _smtpUser = configuration["Smtp:Username"] ?? "";
        private readonly string _smtpPass = configuration["Smtp:Password"] ?? "";

        public async Task SendEmail(string toEmail, string subject, string body)
        {
            using var client = new SmtpClient(_smtpHost, _smtpPort);
            client.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
            client.EnableSsl = true;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUser),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }
    }
}
