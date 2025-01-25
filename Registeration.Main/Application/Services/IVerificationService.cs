using Registeration.Main.Application.Helpers;
using Registeration.Main.Domain.Dtos;
using Registeration.Main.Domain.Models;

namespace Registeration.Main.Application.Services
{
    public interface IVerificationService
    {
        public int GenerateCode();
        public Task<bool> AddVerificationAsync(VerificationCode verification);
        Task<Response<object>> VerifyCodeAsync(VerifyCodeDto dto);
        Task<Response<object>> ResendCodeAsync(ResendCodeDto dto);
        Task<Response<object>> AcceptPolicyAsync(int icNumber);
        Task<Response<object>> AddPinAsync(PinDto dto);
        Task<Response<object>> ToggleBiometricAsync(BiometricDto dto);
    }
}
