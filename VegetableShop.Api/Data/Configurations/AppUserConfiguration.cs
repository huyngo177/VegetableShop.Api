using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VegetableShop.Api.Data.Entities;

namespace VegetableShop.Api.Data.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUsers");

            builder.Property(x => x.FirstName).HasMaxLength(20);

            builder.Property(x => x.LastName).HasMaxLength(20);

            builder.Property(x => x.Address).HasMaxLength(100);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
