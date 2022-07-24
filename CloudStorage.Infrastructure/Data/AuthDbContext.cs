using CloudStorage.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CloudStorage.Infrastructure.Data
{
    public class AuthDbContext : IdentityDbContext<IdentityUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Core.Entities.FileInfo> FileInfos { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<AccountStorage> AccountStorages { get; set; }
    }
}
