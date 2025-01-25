using Registeration.Main.Application.Helpers;

namespace Registeration.Main.Domain.Dtos
{
    public class ResendCodeDto
    {
        public int ICNumber { get; set; }
        public VerificationCodeType Type { get; set; }
    }
}
