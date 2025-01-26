using Registeration.Main.Application.Helpers;
using Registeration.Main.Application.Queues;
using Registeration.Main.Domain.Dtos;
using Registeration.Main.Domain.Models;
using Registeration.Main.Domain.Repos;

namespace Registeration.Main.Application.Services
{
    public class VerificationService(IVerificationCodeRepo verificationRepo, IUserRepo userRepo, VerificationCodeQueue queue) : IVerificationService
    {
        private readonly IVerificationCodeRepo _verificationRepo = verificationRepo;
        private readonly IUserRepo _userRepo = userRepo;
        private readonly VerificationCodeQueue _queue = queue;

        private readonly List<VerificationCodeType> TypesList = [VerificationCodeType.Email, VerificationCodeType.Phone];
        private const int VERIFICATION_CODE_MIN = 1000, VERIFICATION_CODE_MAX = 9999;
        private const int PIN_MIN = 100000, PIN_MAX = 999999;

        public int GenerateCode()
        {
            var random = new Random();
            var code = random.Next(VERIFICATION_CODE_MIN, VERIFICATION_CODE_MAX);
            return code;
        }

        public async Task<bool> AddVerificationAsync(VerificationCode verification)
        {
            await _verificationRepo.AddAsync(verification);
            return await _verificationRepo.CommitAsync();
        }

        public async Task<Response<object>> VerifyCodeAsync(VerifyCodeDto dto)
        {
            if (dto.ICNumber <= 0 || dto.Code < VERIFICATION_CODE_MIN || dto.Code > VERIFICATION_CODE_MAX || !TypesList.Contains(dto.Type))
                return new Response<object>
                {
                    Message = "Invalid data",
                    Status = false
                };

            var verification = await _verificationRepo.GetFirstAsync(dto);
            if (verification == null)
            {
                return new Response<object>
                {
                    Message = "Invalid verification code",
                    Status = false
                };
            }

            verification.IsUsed = true;
            await _verificationRepo.CommitAsync();

            var res = new Response<object>
            {
                Message = "Verification code verified successfully.",
                Status = true
            };

            if (dto.Type == VerificationCodeType.Phone)
            {
                var userCommunicationData = await _userRepo.GetCommunicationDataAsync(dto.ICNumber);

                _queue.Enqueue(new VerificationCodeMessage
                {
                    UserId = verification.UserId,
                    Type = VerificationCodeType.Email,
                    Recipient = userCommunicationData?.Email ?? string.Empty
                });

                res.Message += $" Verification code sent to {userCommunicationData?.Email}.";
            }

            return res;
        }

        public async Task<Response<object>> ResendCodeAsync(ResendCodeDto dto)
        {
            if (dto.ICNumber <= 0 || !TypesList.Contains(dto.Type))
                return new Response<object>
                {
                    Message = "Invalid data",
                    Status = false
                };

            var userCommunicationData = await _userRepo.GetCommunicationDataAsync(dto.ICNumber);
            if (userCommunicationData == null)
            {
                return new Response<object>
                {
                    Message = "Invalid IC Number",
                    Status = false
                };
            }

            _queue.Enqueue(new VerificationCodeMessage
            {
                UserId = userCommunicationData.UserId,
                Type = dto.Type,
                Recipient = dto.Type == VerificationCodeType.Email ? userCommunicationData.Email : userCommunicationData.PhoneNumber
            });

            return new Response<object>
            {
                Message = $"Verification code sent to {(dto.Type == VerificationCodeType.Email ? userCommunicationData.Email : userCommunicationData.PhoneNumber)}.",
                Status = true
            };
        }

        public async Task<Response<object>> AcceptPolicyAsync(int icNumber)
        {
            if(icNumber <= 0)
                return new Response<object>
                {
                    Message = "Invalid IC Number",
                    Status = false
                };
            
            var user = await _userRepo.GetByICNumberAsync(icNumber);
            if (user == null)
            {
                return new Response<object>
                {
                    Message = "Invalid IC Number",
                    Status = false
                };
            }

            user.PrivacyAccepted = true;
            var updated = await _userRepo.CommitAsync();

            if (updated)
                return new Response<object>
                {
                    Message = "Privacy policy accepted successfully.",
                    Status = true
                };

            return new Response<object>
            {
                Message = "Failed to accept privacy policy. Please try again later.",
                Status = false
            };
        }

        public async Task<Response<object>> AddPinAsync(PinDto dto)
        {
            if (dto.ICNumber <= 0 || dto.Pin < PIN_MIN || dto.Pin > PIN_MAX)
                return new Response<object>
                {
                    Message = "Invalid data",
                    Status = false
                };

            var user = await _userRepo.GetByICNumberAsync(dto.ICNumber);
            if (user == null)
            {
                return new Response<object>
                {
                    Message = "Invalid IC Number",
                    Status = false
                };
            }

            user.PinHash = BCrypt.Net.BCrypt.HashPassword(dto.Pin.ToString());
            var updated = await _userRepo.CommitAsync();

            if (updated)
                return new Response<object>
                {
                    Message = "Pin added successfully.",
                    Status = true
                };

            return new Response<object>
            {
                Message = "Failed to add the Pin. Please try again later.",
                Status = false
            };
        }

        public async Task<Response<object>> ToggleBiometricAsync(BiometricDto dto)
        {
            if (dto.ICNumber <= 0)
                return new Response<object>
                {
                    Message = "Invalid IC Number",
                    Status = false
                };

            var user = await _userRepo.GetByICNumberAsync(dto.ICNumber);
            if (user == null)
            {
                return new Response<object>
                {
                    Message = "Invalid IC Number",
                    Status = false
                };
            }

            user.BiometricEnabled = dto.Enabled;
            var updated = await _userRepo.CommitAsync();

            if (updated)
                return new Response<object>
                {
                    Message = $"Biometric login {(dto.Enabled ? "enabled" : "disabled")}.",
                    Status = true
                };

            return new Response<object>
            {
                Message = "Failed to update Biometric login. Please try again later.",
                Status = false
            };
        }
    }
}
