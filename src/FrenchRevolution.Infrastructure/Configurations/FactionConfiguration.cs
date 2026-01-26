using FrenchRevolution.Domain.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrenchRevolution.Infrastructure.Configurations;

internal class FactionConfiguration : IEntityTypeConfiguration<Faction>
{
    public void Configure(EntityTypeBuilder<Faction> builder)
    {
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).ValueGeneratedNever();

        builder.Property(f => f.Title)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(f => f.NormalizedTitle)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(f => f.Description)
            .HasMaxLength(512);

        builder.HasIndex(f => f.NormalizedTitle).IsUnique();

        builder.HasQueryFilter(f => !f.IsDeleted);
    }
}
