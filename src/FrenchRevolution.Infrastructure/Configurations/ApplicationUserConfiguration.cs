using FrenchRevolution.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrenchRevolution.Infrastructure.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(e => e.EnableNotifications)
            .HasDefaultValue(true);

        builder.Property(e => e.DisplayName)
            .HasMaxLength(128);
    }
}