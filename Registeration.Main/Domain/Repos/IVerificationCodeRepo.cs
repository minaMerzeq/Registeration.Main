using Registeration.Main.Domain.Dtos;
using Registeration.Main.Domain.Models;

namespace Registeration.Main.Domain.Repos
{
    public interface IVerificationCodeRepo
    {
        Task AddAsync(VerificationCode verification);
        Task<VerificationCode?> GetFirstAsync(VerifyCodeDto dto);
        Task<bool> CommitAsync();
    }
}
