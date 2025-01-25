using Registeration.Main.Domain.Dtos;
using Registeration.Main.Domain.Models;

namespace Registeration.Main.Domain.Repos
{
    public interface IUserRepo
    {
        Task AddAsync(User user);
        Task<User?> GetByICNumberAsync(int icNumber);
        Task<bool> UserExistsAsync(int icNumber);
        Task<UserCommuncationDto?> GetCommunicationDataAsync(int icNumber);
        Task<bool> CommitAsync();
    }
}
