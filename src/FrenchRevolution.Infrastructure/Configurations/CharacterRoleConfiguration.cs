using FrenchRevolution.Domain.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrenchRevolution.Infrastructure.Configurations;

internal class CharacterRoleConfiguration : IEntityTypeConfiguration<CharacterRole>
{
    public void Configure(EntityTypeBuilder<CharacterRole> builder)
    {
        builder.HasKey(cr => cr.Id);
        builder.Property(cr => cr.Id).ValueGeneratedNever();

        builder.HasOne(cr => cr.Character)
            .WithMany(c => c.CharacterRoles)
            .HasForeignKey(cr => cr.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cr => cr.Role)
            .WithMany(r => r.CharacterRoles)
            .HasForeignKey(cr => cr.RoleId)
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