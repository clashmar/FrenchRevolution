using FrenchRevolution.Domain.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrenchRevolution.Infrastructure.Configurations;

internal class CharacterOfficeConfiguration : IEntityTypeConfiguration<CharacterOffice>
{
    public void Configure(EntityTypeBuilder<CharacterOffice> builder)
    {
        builder.HasKey(cr => cr.Id);
        builder.Property(cr => cr.Id).ValueGeneratedNever();

        builder.HasOne(cr => cr.Character)
            .WithMany(c => c.CharacterOffices)
            .HasForeignKey(cr => cr.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cr => cr.Office)
            .WithMany(r => r.CharacterOffices)
            .HasForeignKey(cr => cr.OfficeId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(cr => cr.From)
            .IsRequired()
            .HasConversion(
                v => v,         
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        builder.Property(cr => cr.To)
            .IsRequired()
            .HasConversion(
                v => v,
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        
        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}