namespace Registeration.Main.Domain.Dtos
{
    public class UserCommuncationDto
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
    }
}
