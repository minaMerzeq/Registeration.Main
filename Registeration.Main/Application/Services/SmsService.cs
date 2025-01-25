using System.IO.Ports;

namespace Registeration.Main.Application.Services
{
    /// <summary>
    /// Use a physical GSM modem or a mobile device connected to the server via USB or serial port
    /// Send SMS using AT commands
    /// </summary>
    public class SmsService : ISmsService
    {
        private readonly string _portName = "COM3"; // Replace with your port
        private readonly int _baudRate = 9600;

        public void SendSms(string toNumber, string message)
        {
            using var port = new SerialPort(_portName, _baudRate);
            port.Open();
            port.WriteLine("AT\r"); // Test command
            port.WriteLine("AT+CMGF=1\r"); // Set to text mode
            port.WriteLine($"AT+CMGS=\"{toNumber}\"\r"); // Specify phone number
            port.WriteLine($"{message}\x1A"); // Message and End of Message signal
        }
    }
}
