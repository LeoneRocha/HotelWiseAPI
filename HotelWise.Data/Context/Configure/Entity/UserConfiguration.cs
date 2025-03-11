using HotelWise.Data.Context.Configure.Helper;
using HotelWise.Data.Context.Configure.Mock;
using HotelWise.Domain.Constants;
using HotelWise.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelWise.Data.Context.Configure.Entity
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            HelperCharSet.AddCharSet(builder);
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Enable);
            builder.Property(e => e.Name).HasMaxLength(255).IsRequired().HasColumnType(EntityTypeConfigurationConstants.Type_Varchar_255);

            builder.Property(e => e.Email).HasMaxLength(100).IsRequired().HasColumnType("varchar(100)");
            builder.Property(e => e.Login).HasMaxLength(25).IsRequired().HasColumnType("varchar(25)");
            builder.Property(e => e.PasswordHash);
            builder.Property(e => e.PasswordSalt);
            builder.Property(e => e.Role).HasMaxLength(50).IsRequired().HasColumnType("varchar(50)");
            builder.Property(e => e.Admin);
            builder.Property(e => e.Language).HasMaxLength(10).HasColumnType("varchar(10)");
            builder.Property(e => e.TimeZone).HasMaxLength(255).HasColumnType(EntityTypeConfigurationConstants.Type_Varchar_255);
            builder.Property(e => e.RefreshToken);
            builder.Property(e => e.RefreshTokenExpiryTime).HasColumnName("Refresh_token_expiry_time");

            builder.HasData(UserMockData.GetMock());
        }
    }
}
