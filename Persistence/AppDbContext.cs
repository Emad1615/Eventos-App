using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Persistence
{
    public class AppDbContext : IdentityDbContext<UserApplication>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Activity> activities { get; set; }
        public DbSet<RefreshToken> refreshTokens { get; set; }
    }
}
