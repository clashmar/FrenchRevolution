using FrenchRevolution.Domain.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrenchRevolution.Infrastructure.Configurations;

internal class OfficeConfiguration : IEntityTypeConfiguration<Office>
{
    public void Configure(EntityTypeBuilder<Office> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever();

        builder.Property(r => r.Title)
            .IsRequired()
            .HasMaxLength(128);
        
        builder.Property(r => r.NormalizedTitle)
            .IsRequired()
            .HasMaxLength(128);
        
        builder.HasIndex(r => r.NormalizedTitle).IsUnique();

        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}