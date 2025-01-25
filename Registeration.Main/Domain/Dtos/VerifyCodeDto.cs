namespace Registeration.Main.Domain.Dtos
{
    public class VerifyCodeDto : ResendCodeDto
    {
        public int Code { get; set; }
    }
}
