using Microsoft.EntityFrameworkCore;
using Registeration.Main.Domain.Dtos;
using Registeration.Main.Domain.Models;

namespace Registeration.Main.Domain.Repos
{
    public class UserRepo(AppDbContext context) : IUserRepo
    {
        private readonly AppDbContext _context = context;

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<User?> GetByICNumberAsync(int icNumber)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.ICNumber == icNumber);
        }

        public async Task<bool> UserExistsAsync(int icNumber)
        {
            return await _context.Users.AnyAsync(x => x.ICNumber == icNumber);
        }

        public async Task<UserCommuncationDto?> GetCommunicationDataAsync(int icNumber)
        {
            return await _context.Users.Where(x => x.ICNumber == icNumber)
                .Select(x => new UserCommuncationDto
                {
                    UserId = x.Id,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CommitAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
