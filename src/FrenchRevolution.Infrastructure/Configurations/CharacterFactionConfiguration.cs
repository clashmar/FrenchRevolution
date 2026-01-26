using FrenchRevolution.Domain.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrenchRevolution.Infrastructure.Configurations;

internal class CharacterFactionConfiguration : IEntityTypeConfiguration<CharacterFaction>
{
    public void Configure(EntityTypeBuilder<CharacterFaction> builder)
    {
        builder.HasKey(cf => cf.Id);
        builder.Property(cf => cf.Id).ValueGeneratedNever();

        builder.HasOne(cf => cf.Character)
            .WithMany(c => c.CharacterFactions)
            .HasForeignKey(cf => cf.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cf => cf.Faction)
            .WithMany(f => f.CharacterFactions)
            .HasForeignKey(cf => cf.FactionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(cf => !cf.IsDeleted);
    }
}
