using CloudStorage.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudStorage.Infrastructure.Data.Config
{
    public class AccountStorageConfiguration : IEntityTypeConfiguration<AccountStorage>
    {
        public void Configure(EntityTypeBuilder<AccountStorage> builder)
        {
            builder.HasKey(x => x.UserId);
        }
    }
}
