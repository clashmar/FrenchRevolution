using FrenchRevolution.Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace FrenchRevolution.Infrastructure.Data;

internal static class SeedData
{
    private static readonly Character[] Characters =
    [
        new(
            "Maximilien Robespierre",
            "Lawyer",
            new DateTime(1758, 5, 6),
            new DateTime(1794, 7, 28)),

        new(
            "Georges Jacques Danton",
            "Lawyer",
            new DateTime(1759, 10, 26),
            new DateTime(1794, 4, 5)),
        
        new(
            "Camilles Desmoulins",
            "Journalist",
            new DateTime(1760, 3, 2),
            new DateTime(1794, 4, 5))
    ];
    
    public static void SeedStaticData(AppDbContext context)
    {
        if (context.Characters.Any())
        {
            return;
        }

        context.Characters.AddRange(Characters);
    }

    public static async Task SeedStaticDataAsync(
        AppDbContext context,
        CancellationToken cancellationToken = default)
    {
        if (await context.Characters.AnyAsync(cancellationToken))
        {
            return;
        }
        
        await context.Characters.AddRangeAsync(Characters, cancellationToken);
    }
}