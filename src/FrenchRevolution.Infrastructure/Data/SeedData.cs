using FrenchRevolution.Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace FrenchRevolution.Infrastructure.Data;

internal static class SeedData
{
    private static Role[] CreateRoles() =>
    [
        new("President of the Committee of Public Safety"), // 0
        new("Member of the Committee of Public Safety"), // 1
        new("President of the National Convention"), // 2
        new("Deputy of the National Convention"), // 3
        new("Minister of Justice") // 4
    ];

    private static Character[] CreateCharacters(Role[] roles)
    {
        var robespierre = new Character(
            "Maximilien Robespierre",
            "Lawyer",
            new DateTime(1758, 5, 6),
            new DateTime(1794, 7, 28));
        
        robespierre.AssignRole(roles[2], new DateTime(1793, 8, 22), new DateTime(1793, 9, 7));
        robespierre.AssignRole(roles[0], new DateTime(1793, 6, 4), new DateTime(1794, 6, 19));

        var danton = new Character(
            "Georges Jacques Danton",
            "Lawyer",
            new DateTime(1759, 10, 26),
            new DateTime(1794, 4, 5));
        
        danton.AssignRole(roles[1], new DateTime(1793, 4, 6), new DateTime(1793, 7, 10));
        danton.AssignRole(roles[4], new DateTime(1792, 8, 10), new DateTime(1792, 10, 9));

        var desmoulins = new Character(
            "Camille Desmoulins",
            "Journalist",
            new DateTime(1760, 3, 2),
            new DateTime(1794, 4, 5));
        
        desmoulins.AssignRole(roles[3], new DateTime(1792, 9, 20), new DateTime(1794, 4, 5));

        return [
            robespierre, 
            danton, 
            desmoulins
        ];
    }

    public static void SeedStaticData(AppDbContext context)
    {
        if (context.Roles.Any() || context.Characters.Any())
        {
            return;
        }

        var roles = CreateRoles();
        context.Roles.AddRange(roles);
        
        var characters = CreateCharacters(roles);
        context.Characters.AddRange(characters);
    }

    public static async Task SeedStaticDataAsync(
        AppDbContext context,
        CancellationToken ct = default)
    {
        if (await context.Roles.AnyAsync(ct) || await context.Characters.AnyAsync(ct))
        {
            return;
        }

        var roles = CreateRoles();
        await context.Roles.AddRangeAsync(roles, ct);
        
        var characters = CreateCharacters(roles);
        await context.Characters.AddRangeAsync(characters, ct);
    }
}