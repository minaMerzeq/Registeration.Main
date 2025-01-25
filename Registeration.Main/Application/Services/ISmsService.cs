namespace Registeration.Main.Application.Services
{
    public interface ISmsService
    {
        public void SendSms(string toNumber, string message);
    }
}
