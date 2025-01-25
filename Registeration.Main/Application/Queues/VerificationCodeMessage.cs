using Registeration.Main.Application.Helpers;

namespace Registeration.Main.Application.Queues
{
    public class VerificationCodeMessage
    {
        public int UserId { get; set; }
        public VerificationCodeType Type { get; set; }
        public string Recipient { get; set; } = null!;
    }
}
