namespace Registeration.Main.Application.Services
{
    public interface IEmailService
    {
        public Task SendEmail(string toEmail, string subject, string body);
    }
}
