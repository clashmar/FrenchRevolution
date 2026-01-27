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
        new("Minister of Justice"), // 4
        new("King of France"), // 5
        new("Queen of France"), // 6
        new("Commander of the National Guard"), // 7
        new("Substitute Procurator of the Paris Commune"), // 8
        new("Minister of the Interior") // 9
    ];

    private static Faction[] CreateFactions() =>
    [
        new("Jacobins", "Radical republican movement centered at the Jacobin Club."), // 0
        new("Girondins", "Moderate republican faction associated with the Gironde department."), // 1
        new("Dantonists", "Followers of Georges Danton advocating for moderation."), // 2
        new("Montagnards", "Radical faction seated on the high benches of the Convention."), // 3
        new("Cordeliers", "Popular democratic club known for radical politics."), // 4
        new("Royalists", "Supporters of the monarchy and traditional order."), // 5
        new("Enragés", "Ultra-radical faction demanding economic controls."), // 6
        new("Feuillants", "Constitutional monarchist club advocating for a constitutional monarchy."), // 7
        new("Hébertists", "Ultra-radical followers of Jacques Hébert.") // 8
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
            new Portrait("https://upload.wikimedia.org/wikipedia/commons/e/e4/AduC_061_Georges_Jacques_Danton_%28G.J.%2C_1759-1794%29.jpg"));

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

        var saintJust = new Character(
            "Louis Antoine de Saint-Just",
            "Politician",
            new DateTime(1767, 8, 25),
            new DateTime(1794, 7, 28),
            new Portrait("https://upload.wikimedia.org/wikipedia/commons/5/59/Saint_Just.jpg"));

        saintJust.AssignOffice(offices[1], new DateTime(1793, 5, 30), new DateTime(1794, 7, 28));
        saintJust.AssignFaction(factions[0]);
        saintJust.AssignFaction(factions[3]);

        var marat = new Character(
            "Jean-Paul Marat",
            "Journalist",
            new DateTime(1743, 5, 24),
            new DateTime(1793, 7, 13),
            new Portrait("https://upload.wikimedia.org/wikipedia/commons/3/36/Jean-Paul_Marat_-_portrait_peint_par_Joseph_Boze.jpg"));

        marat.AssignOffice(offices[3], new DateTime(1792, 9, 20), new DateTime(1793, 7, 13));
        marat.AssignFaction(factions[0]);
        marat.AssignFaction(factions[3]);
        marat.AssignFaction(factions[4]);

        var hebert = new Character(
            "Jacques Hébert",
            "Journalist",
            new DateTime(1757, 11, 15),
            new DateTime(1794, 3, 24),
            new Portrait("https://upload.wikimedia.org/wikipedia/commons/a/a1/Jacques_Ren%C3%A9_H%C3%A9bert.JPG"));

        hebert.AssignOffice(offices[8], new DateTime(1792, 12, 1), new DateTime(1794, 3, 24));
        hebert.AssignFaction(factions[4]);
        hebert.AssignFaction(factions[6]);
        hebert.AssignFaction(factions[8]);

        var lafayette = new Character(
            "Gilbert du Motier, Marquis de Lafayette",
            "General",
            new DateTime(1757, 9, 6),
            new DateTime(1834, 5, 20),
            new Portrait("https://upload.wikimedia.org/wikipedia/commons/5/52/Gilbert_du_Motier_Marquis_de_Lafayette.jpg"));

        lafayette.AssignOffice(offices[7], new DateTime(1789, 7, 15), new DateTime(1791, 10, 8));
        lafayette.AssignFaction(factions[7]);

        var couthon = new Character(
            "Georges Couthon",
            "Lawyer",
            new DateTime(1755, 12, 22),
            new DateTime(1794, 7, 28),
            new Portrait("https://upload.wikimedia.org/wikipedia/commons/e/e8/Fran%C3%A7ois_Bonneville_-_Portrait_pr%C3%A9sum%C3%A9_de_Georges_Couthon_%281755-1794%29%2C_conventionnel_-_P16_-_Mus%C3%A9e_Carnavalet.jpg"));

        couthon.AssignOffice(offices[1], new DateTime(1793, 5, 30), new DateTime(1794, 7, 28));
        couthon.AssignFaction(factions[0]);
        couthon.AssignFaction(factions[3]);

        var louisXVI = new Character(
            "Louis XVI",
            "Monarch",
            new DateTime(1754, 8, 23),
            new DateTime(1793, 1, 21),
            new Portrait("https://upload.wikimedia.org/wikipedia/commons/2/2e/Ludvig_XVI_av_Frankrike_portr%C3%A4tterad_av_AF_Callet.jpg"));

        louisXVI.AssignOffice(offices[5], new DateTime(1774, 5, 10), new DateTime(1792, 9, 21));
        louisXVI.AssignFaction(factions[5]);

        var marieAntoinette = new Character(
            "Marie Antoinette",
            "Monarch",
            new DateTime(1755, 11, 2),
            new DateTime(1793, 10, 16),
            new Portrait("https://upload.wikimedia.org/wikipedia/commons/f/f6/Joseph_Ducreux_-_Portrait_of_Marie_Antoinette_of_Austria%2C_the_later_queen_of_France.jpg"));

        marieAntoinette.AssignOffice(offices[6], new DateTime(1774, 5, 10), new DateTime(1792, 9, 21));
        marieAntoinette.AssignFaction(factions[5]);

        var david = new Character(
            "Jacques-Louis David",
            "Painter",
            new DateTime(1748, 8, 30),
            new DateTime(1825, 12, 29),
            new Portrait("https://upload.wikimedia.org/wikipedia/commons/c/c6/David_Self_Portrait.jpg"));

        david.AssignOffice(offices[3], new DateTime(1792, 9, 20), new DateTime(1795, 10, 26));
        david.AssignFaction(factions[0]);
        david.AssignFaction(factions[3]);

        var carnot = new Character(
            "Lazare Carnot",
            "Military Engineer",
            new DateTime(1753, 5, 13),
            new DateTime(1823, 8, 2),
            new Portrait("https://upload.wikimedia.org/wikipedia/commons/a/a4/Lazare-Carnot-par-Boilly.jpg"));

        carnot.AssignOffice(offices[1], new DateTime(1793, 8, 14), new DateTime(1795, 3, 6));
        carnot.AssignFaction(factions[3]);

        var barere = new Character(
            "Bertrand Barère",
            "Lawyer",
            new DateTime(1755, 9, 10),
            new DateTime(1841, 1, 13),
            new Portrait("https://upload.wikimedia.org/wikipedia/commons/7/75/Barere.jpg"));

        barere.AssignOffice(offices[1], new DateTime(1793, 4, 6), new DateTime(1794, 7, 27));
        barere.AssignOffice(offices[2], new DateTime(1793, 1, 21), new DateTime(1793, 2, 4));
        barere.AssignFaction(factions[3]);

        var brissot = new Character(
            "Jacques Pierre Brissot",
            "Journalist",
            new DateTime(1754, 1, 15),
            new DateTime(1793, 10, 31),
            new Portrait("https://upload.wikimedia.org/wikipedia/commons/f/f2/Fran%C3%A7ois_Bonneville_-_Portrait_de_Jacques-Pierre_Brissot_de_Warville_%281754-1793%29%2C_journaliste_et_conventionnel_-_P2608_-_Mus%C3%A9e_Carnavalet.jpg"));

        brissot.AssignOffice(offices[3], new DateTime(1791, 10, 1), new DateTime(1793, 6, 2));
        brissot.AssignFaction(factions[1]);

        var vergniaud = new Character(
            "Pierre Vergniaud",
            "Lawyer",
            new DateTime(1753, 5, 31),
            new DateTime(1793, 10, 31),
            new Portrait("https://upload.wikimedia.org/wikipedia/commons/5/57/AduC_132_Pierre_Vergniaud_%28P.V.%2C_1758-1793%29.jpg"));

        vergniaud.AssignOffice(offices[3], new DateTime(1791, 10, 1), new DateTime(1793, 6, 2));
        vergniaud.AssignOffice(offices[2], new DateTime(1792, 1, 17), new DateTime(1792, 1, 31));
        vergniaud.AssignFaction(factions[1]);

        var roland = new Character(
            "Jean-Marie Roland",
            "Politician",
            new DateTime(1734, 2, 18),
            new DateTime(1793, 11, 15),
            new Portrait("https://upload.wikimedia.org/wikipedia/commons/4/48/AduC_044_Roland_%28J.M.%2C_1734-1793%29.JPG"));

        roland.AssignOffice(offices[9], new DateTime(1792, 3, 23), new DateTime(1792, 6, 13));
        roland.AssignOffice(offices[9], new DateTime(1792, 8, 10), new DateTime(1793, 1, 22));
        roland.AssignFaction(factions[1]);

        var philippeEgalite = new Character(
            "Philippe Égalité",
            "Duke",
            new DateTime(1747, 4, 13),
            new DateTime(1793, 11, 6),
            new Portrait("https://upload.wikimedia.org/wikipedia/commons/0/0f/Duke_Louis_Philippe_I_of_Orl%C3%A9ans.jpg"));

        philippeEgalite.AssignOffice(offices[3], new DateTime(1792, 9, 20), new DateTime(1793, 4, 6));
        philippeEgalite.AssignFaction(factions[0]);

        var tallien = new Character(
            "Jean-Lambert Tallien",
            "Politician",
            new DateTime(1767, 1, 23),
            new DateTime(1820, 11, 16),
            new Portrait("https://upload.wikimedia.org/wikipedia/commons/a/a8/Jean_Lambert_Tallien.JPG"));

        tallien.AssignOffice(offices[3], new DateTime(1792, 9, 20), new DateTime(1795, 10, 26));
        tallien.AssignOffice(offices[2], new DateTime(1794, 3, 22), new DateTime(1794, 4, 5));
        tallien.AssignFaction(factions[3]);

        var barras = new Character(
            "Paul Barras",
            "Politician",
            new DateTime(1755, 6, 30),
            new DateTime(1829, 1, 29),
            new Portrait("https://upload.wikimedia.org/wikipedia/commons/7/7c/Paul_Barras.jpg"));

        barras.AssignOffice(offices[3], new DateTime(1792, 9, 20), new DateTime(1795, 10, 26));
        barras.AssignOffice(offices[2], new DateTime(1793, 2, 5), new DateTime(1793, 2, 19));
        barras.AssignFaction(factions[3]);

        var barnave = new Character(
            "Antoine Barnave",
            "Lawyer",
            new DateTime(1761, 10, 22),
            new DateTime(1793, 11, 29),
            new Portrait("https://upload.wikimedia.org/wikipedia/commons/5/5e/Antoine-Pierre-Joseph-Marie_Barnave.png"));

        barnave.AssignOffice(offices[3], new DateTime(1789, 5, 5), new DateTime(1791, 9, 30));
        barnave.AssignFaction(factions[0]);
        barnave.AssignFaction(factions[7]);

        return [
            robespierre,
            danton,
            desmoulins,
            saintJust,
            marat,
            hebert,
            lafayette,
            couthon,
            louisXVI,
            marieAntoinette,
            david,
            carnot,
            barere,
            brissot,
            vergniaud,
            roland,
            philippeEgalite,
            tallien,
            barras,
            barnave
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