using Registeration.Main.Application.Helpers;

namespace Registeration.Main.Domain.Models
{
    public class VerificationCode
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Code { get; set; }
        public VerificationCodeType Type { get; set; }
        public DateTime Expiry { get; set; } = DateTime.UtcNow.AddMinutes(2);
        public bool IsUsed { get; set; } = false;

        // Navigation Property
        public virtual User User { get; set; } = null!;
    }
}
