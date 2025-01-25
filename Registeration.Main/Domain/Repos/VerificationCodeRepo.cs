using Microsoft.EntityFrameworkCore;
using Registeration.Main.Domain.Dtos;
using Registeration.Main.Domain.Models;

namespace Registeration.Main.Domain.Repos
{
    public class VerificationCodeRepo(AppDbContext context) : IVerificationCodeRepo
    {
        private readonly AppDbContext _context = context;

        public async Task AddAsync(VerificationCode verification)
        {
            await _context.VerificationCodes.AddAsync(verification);
        }

        public async Task<bool> CommitAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public Task<VerificationCode?> GetFirstAsync(VerifyCodeDto dto)
        {
            return _context.VerificationCodes.FirstOrDefaultAsync(x => x.User.ICNumber == dto.ICNumber && x.Code == dto.Code && x.Type == dto.Type && 
                    x.Expiry >= DateTime.UtcNow && x.IsUsed == false);
        }
    }
}
