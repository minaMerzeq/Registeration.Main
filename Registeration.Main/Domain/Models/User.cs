namespace Registeration.Main.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public int ICNumber { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool IsEmailVerified { get; set; } = false;
        public bool IsPhoneVerified { get; set; } = false;
        public bool PrivacyAccepted { get; set; } = false;
        public string? PinHash { get; set; }
        public bool BiometricEnabled { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Property
        public ICollection<VerificationCode> VerificationCodes { get; set; } = [];
    }
}
