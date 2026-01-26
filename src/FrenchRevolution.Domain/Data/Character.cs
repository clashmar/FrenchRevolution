using FrenchRevolution.Domain.Exceptions;
using FrenchRevolution.Domain.Primitives;

namespace FrenchRevolution.Domain.Data;

public sealed class Character : Entity
{
    public string Name { get; private set; }
    public string NormalizedName { get; private set; }
    public string Profession { get; private set; }
    public List<CharacterOffice> CharacterOffices { get; private set; } = [];
    public List<CharacterFaction> CharacterFactions { get; private set; } = [];
    public DateTime Born { get; private set; }
    public DateTime Died { get; private set; }

    public Portrait Portrait { get; private set; }
    
    private Character() { }
    
    public Character(
        string name, 
        string profession,
        DateTime born,
        DateTime died,
        Portrait portrait
        ) : base(Guid.NewGuid())
    {
        ValidateRequiredProperty(name, nameof(name));
        ValidateRequiredProperty(profession, nameof(profession));
        ValidateLifeSpan(born, died);
        
        Name = name;
        NormalizedName = name.Trim().ToLowerInvariant();
        Profession = profession;
        Born = DateTime.SpecifyKind(born, DateTimeKind.Utc);
        Died = DateTime.SpecifyKind(died, DateTimeKind.Utc);
        Portrait = portrait;
    }

    public void Update(
        string name, 
        string profession, 
        DateTime born, 
        DateTime died,
        Portrait portrait
        )
    {
        ValidateRequiredProperty(name, nameof(name));
        ValidateRequiredProperty(profession, nameof(profession));
        ValidateLifeSpan(born, died);
        
        Name = name;
        NormalizedName = name.Trim().ToLowerInvariant();
        Profession = profession;
        Born = DateTime.SpecifyKind(born, DateTimeKind.Utc);
        Died = DateTime.SpecifyKind(died, DateTimeKind.Utc);
        Portrait = portrait;
    }

    public void AssignOffice(Office office, DateTime from, DateTime to)
    {
        if (from > to)
        {
            throw new InvalidTimeSpanException(from, to);
        }

        var characterRole = new CharacterOffice(Id, office.Id, from, to);
        CharacterOffices.Add(characterRole);
    }

    public void ClearOffices()
    {
        CharacterOffices.Clear();
    }

    public void UpdateOffices(IEnumerable<CharacterOffice> newOffices)
    {
        CharacterOffices.Clear();
        CharacterOffices.AddRange(newOffices);
    }

    public void AssignFaction(Faction faction)
    {
        var characterFaction = new CharacterFaction(Id, faction.Id);
        CharacterFactions.Add(characterFaction);
    }

    public void ClearFactions()
    {
        CharacterFactions.Clear();
    }

    private static void ValidateRequiredProperty(string value, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidPropertyException(propertyName);
        }
    }

    private static void ValidateLifeSpan(DateTime born, DateTime died)
    {
        if (born > died)
        {
            throw new InvalidLifeSpanException(born, died);
        }
    }
}