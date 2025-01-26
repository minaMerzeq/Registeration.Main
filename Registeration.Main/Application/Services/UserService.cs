using Registeration.Main.Application.Helpers;
using Registeration.Main.Application.Queues;
using Registeration.Main.Domain.Dtos;
using Registeration.Main.Domain.Models;
using Registeration.Main.Domain.Repos;

namespace Registeration.Main.Application.Services
{
    public class UserService(IUserRepo userRepo, VerificationCodeQueue queue) : IUserService
    {
        private readonly IUserRepo _userRepo = userRepo;
        private readonly VerificationCodeQueue _queue = queue;

        public async Task<Response<RegisterResDto>> RegisterAsync(RegisterUserDto dto)
        {
            if(dto.ICNumber <= 0 || string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.PhoneNumber))
                return new Response<RegisterResDto>
                {
                    Status = false,
                    Message = "Invalid data",
                };
            
            var isUserExists = await _userRepo.UserExistsAsync(dto.ICNumber);
            if (isUserExists) 
                return new Response<RegisterResDto>
                {
                    Status = false,
                    Data = new RegisterResDto { AccountExist = true },
                    Message = "Account already exist",
                };

            var user = new User
            {
                ICNumber = dto.ICNumber,
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            await _userRepo.AddAsync(user);
            var added = await _userRepo.CommitAsync();

            if (added)
            {
                _queue.Enqueue(new VerificationCodeMessage
                {
                    UserId = user.Id,
                    Type = VerificationCodeType.Email,
                    Recipient = user.Email,
                });

                return new Response<RegisterResDto>
                {
                    Status = true,
                    Message = $"User registered successfully. Verification code sent to {dto.PhoneNumber}.",
                };
            }

            return new Response<RegisterResDto>
            {
                Status = false,
                Message = "Failed to register user. Please try again later.",
            };
        }

        public async Task<Response<UserCommuncationDto>> LoginAsync(int ICNumber)
        {
            if (ICNumber <= 0)
                return new Response<UserCommuncationDto>
                {
                    Status = false,
                    Message = "Invalid IC Number",
                };

            var userCommunicationData = await _userRepo.GetCommunicationDataAsync(ICNumber);
            if (userCommunicationData == null)
                return new Response<UserCommuncationDto>
                {
                    Status = false,
                    Message = "Account not found",
                };

            _queue.Enqueue(new VerificationCodeMessage
            {
                UserId = userCommunicationData.UserId,
                Type = VerificationCodeType.Phone,
                Recipient = userCommunicationData.PhoneNumber,
            });

            return new Response<UserCommuncationDto>
            {
                Status = true,
                Data = userCommunicationData,
                Message = $"User logged in. Verification code sent to {userCommunicationData.PhoneNumber}",
            };
        }
    }
}
