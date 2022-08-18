using CloudStorage.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudStorage.Infrastructure.Data.Config
{
    public class AccountStorageConfiguration : IEntityTypeConfiguration<AccountExtension>
    {
        public void Configure(EntityTypeBuilder<AccountExtension> builder)
        {
            builder.HasKey(x => x.UserId);
        }
    }
}
