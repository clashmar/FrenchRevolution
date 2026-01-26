using FrenchRevolution.Domain.Data;
using FrenchRevolution.Domain.Primitives;
using Microsoft.EntityFrameworkCore;

namespace FrenchRevolution.Infrastructure.Data;

internal static class SeedData
{
    private static Office[] CreateOffices() =>
    [
        new("President of the Committee of Public Safety"), // 0
        new("Member of the Committee of Public Safety"), // 1
        new("President of the National Convention"), // 2
        new("Deputy of the National Convention"), // 3
        new("Minister of Justice") // 4
    ];

    private static Faction[] CreateFactions() =>
    [
        new("Jacobins", "Radical republican movement centered at the Jacobin Club"), // 0
        new("Girondins", "Moderate republican faction associated with the Gironde department"), // 1
        new("Dantonists", "Followers of Georges Danton advocating for moderation"), // 2
        new("Montagnards", "Radical faction seated on the high benches of the Convention"), // 3
        new("Cordeliers", "Popular democratic club known for radical politics"), // 4
        new("Royalists", "Supporters of the monarchy and traditional order"), // 5
        new("Enragés", "Ultra-radical faction demanding economic controls") // 6
    ];

    private static Character[] CreateCharacters(Office[] offices, Faction[] factions)
    {
        var robespierre = new Character(
            "Maximilien Robespierre",
            "Lawyer",
            new DateTime(1758, 5, 6),
            new DateTime(1794, 7, 28),
            new Portrait("https://upload.wikimedia.org/wikipedia/commons/1/12/Robespierre.jpg"));

        robespierre.AssignOffice(offices[2], new DateTime(1793, 8, 22), new DateTime(1793, 9, 7));
        robespierre.AssignOffice(offices[0], new DateTime(1793, 6, 4), new DateTime(1794, 6, 19));
        robespierre.AssignFaction(factions[0]);
        robespierre.AssignFaction(factions[3]); 

        var danton = new Character(
            "Georges Jacques Danton",
            "Lawyer",
            new DateTime(1759, 10, 26),
            new DateTime(1794, 4, 5),
            new Portrait("https://upload.wikimedia.org/wikipedia/commons/4/4d/Camille_Desmoulins.jpg"));

        danton.AssignOffice(offices[1], new DateTime(1793, 4, 6), new DateTime(1793, 7, 10));
        danton.AssignOffice(offices[4], new DateTime(1792, 8, 10), new DateTime(1792, 10, 9));
        danton.AssignFaction(factions[0]); 
        danton.AssignFaction(factions[4]);
        danton.AssignFaction(factions[2]);

        var desmoulins = new Character(
            "Camille Desmoulins",
            "Journalist",
            new DateTime(1760, 3, 2),
            new DateTime(1794, 4, 5),
            new Portrait("https://upload.wikimedia.org/wikipedia/commons/4/4d/Camille_Desmoulins.jpg"));

        desmoulins.AssignOffice(offices[3], new DateTime(1792, 9, 20), new DateTime(1794, 4, 5));
        desmoulins.AssignFaction(factions[0]); 
        desmoulins.AssignFaction(factions[4]); 
        desmoulins.AssignFaction(factions[2]); 

        return [
            robespierre,
            danton,
            desmoulins
        ];
    }

    public static void SeedStaticData(AppDbContext context)
    {
        if (context.Offices.Any() || context.Characters.Any())
        {
            return;
        }

        var offices = CreateOffices();
        context.Offices.AddRange(offices);

        var factions = CreateFactions();
        context.Factions.AddRange(factions);

        var characters = CreateCharacters(offices, factions);
        context.Characters.AddRange(characters);
    }

    public static async Task SeedStaticDataAsync(
        AppDbContext context,
        CancellationToken ct = default)
    {
        if (await context.Offices.AnyAsync(ct) || await context.Characters.AnyAsync(ct))
        {
            return;
        }

        var offices = CreateOffices();
        await context.Offices.AddRangeAsync(offices, ct);

        var factions = CreateFactions();
        await context.Factions.AddRangeAsync(factions, ct);

        var characters = CreateCharacters(offices, factions);
        await context.Characters.AddRangeAsync(characters, ct);
    }
}