using FrenchRevolution.Domain.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrenchRevolution.Infrastructure.Configurations;

internal class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
            .ValueGeneratedNever();
        
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(128);
        
        builder.Property(p => p.Profession)
            .IsRequired()
            .HasMaxLength(128);
        
        builder.Property(c => c.DateOfBirth)
            .IsRequired()
            .HasConversion(
                v => v,         
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        builder.Property(c => c.DateOfDeath)
            .IsRequired()
            .HasConversion(
                v => v,
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        
        builder.HasMany(c => c.CharacterRoles)
            .WithOne(cr => cr.Character)
            .HasForeignKey(cr => cr.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasQueryFilter(p => !p.IsDeleted); 
    }
}