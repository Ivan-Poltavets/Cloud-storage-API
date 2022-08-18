using CloudStorage.Core.Entities;
using CloudStorage.Infrastructure.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace CloudStorage.Infrastructure.Data
{
    public class CosmosDbContext : DbContext
    {
        public DbSet<Core.Entities.FileInfo> FileInfos { get; set; }
        public DbSet<FolderInfo> Folders { get; set; }
        public DbSet<AccountExtension> AccountStorages { get; set; }

        public CosmosDbContext(DbContextOptions<CosmosDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new AccountStorageConfiguration());
        }
    }
}
