using FrenchRevolution.Domain.Exceptions;
using FrenchRevolution.Domain.Primitives;

namespace FrenchRevolution.Domain.Data;

public sealed class Character : Entity
{ 
    public string Name { get; private set; }
    public string Profession { get; private set; }
    public List<CharacterOffice> CharacterOffices { get; private set; } = [];
    public DateTime Born { get; private set; }
    public DateTime Died { get; private set; }
    
    
    public Character(
        string name, 
        string profession,
        DateTime born,
        DateTime died
        ) : base(Guid.NewGuid())
    {
        ValidateRequiredProperty(name, nameof(name));
        ValidateRequiredProperty(profession, nameof(profession));
        ValidateLifeSpan(born, died);
        
        Name = name;
        Profession = profession;
        Born = DateTime.SpecifyKind(born, DateTimeKind.Utc);
        Died = DateTime.SpecifyKind(died, DateTimeKind.Utc);
    }

    public void Update(
        string name, 
        string profession, 
        DateTime born, 
        DateTime died
        )
    {
        ValidateRequiredProperty(name, nameof(name));
        ValidateRequiredProperty(profession, nameof(profession));
        ValidateLifeSpan(born, died);
        
        Name = name;
        Profession = profession;
        Born = DateTime.SpecifyKind(born, DateTimeKind.Utc);
        Died = DateTime.SpecifyKind(died, DateTimeKind.Utc);
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