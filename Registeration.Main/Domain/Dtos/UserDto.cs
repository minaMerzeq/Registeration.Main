namespace Registeration.Main.Domain.Dtos
{
    public class RegisterUserDto
    {
        public int UserId { get; set; }
        public int ICNumber { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
    }
}
