using Registeration.Main.Application.Helpers;
using Registeration.Main.Domain.Dtos;

namespace Registeration.Main.Application.Services
{
    public interface IUserService
    {
        Task<Response<RegisterResDto>> RegisterAsync(RegisterUserDto user);
        Task<Response<UserCommuncationDto>> LoginAsync(int ICNumber);
    }
}
