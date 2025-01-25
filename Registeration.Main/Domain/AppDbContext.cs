using Microsoft.EntityFrameworkCore;
using Registeration.Main.Domain.Models;

namespace Registeration.Main.Domain
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<VerificationCode> VerificationCodes { get; set; } = null!;
    }
}
