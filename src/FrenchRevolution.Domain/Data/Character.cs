using FrenchRevolution.Domain.Exceptions;
using FrenchRevolution.Domain.Primitives;

namespace FrenchRevolution.Domain.Data;

public sealed class Character : Entity
{ 
    public string Name { get; private set; }
    public string Profession { get; private set; }
    public List<CharacterRole> CharacterRoles { get; private set; } = [];
    public DateTime DateOfBirth { get; private set; }
    public DateTime DateOfDeath { get; private set; }
    
    
    public Character(
        string name, 
        string profession,
        DateTime dateOfBirth,
        DateTime dateOfDeath
        ) : base(Guid.NewGuid())
    {
        ValidateRequiredProperty(name, nameof(name));
        ValidateRequiredProperty(profession, nameof(profession));
        ValidateLifeSpan(dateOfBirth, dateOfDeath);
        
        Name = name;
        Profession = profession;
        DateOfBirth = DateTime.SpecifyKind(dateOfBirth, DateTimeKind.Utc);
        DateOfDeath = DateTime.SpecifyKind(dateOfDeath, DateTimeKind.Utc);
    }

    public void Update(
        string name, 
        string profession, 
        DateTime dateOfBirth, 
        DateTime dateOfDeath
        )
    {
        ValidateRequiredProperty(name, nameof(name));
        ValidateRequiredProperty(profession, nameof(profession));
        ValidateLifeSpan(dateOfBirth, dateOfDeath);
        
        Name = name;
        Profession = profession;
        DateOfBirth = DateTime.SpecifyKind(dateOfBirth, DateTimeKind.Utc);
        DateOfDeath = DateTime.SpecifyKind(dateOfDeath, DateTimeKind.Utc);
    }
    
    public void AssignRole(Role role, DateTime from, DateTime to)
    {
        if (from > to)
        {
            throw new InvalidTimeSpanException(from, to);
        }

        var characterRole = new CharacterRole(Id, role.Id, from, to);
        CharacterRoles.Add(characterRole);
    }

    public void ClearRoles()
    {
        CharacterRoles.Clear();
    }

    public void UpdateRoles(IEnumerable<CharacterRole> newRoles)
    {
        CharacterRoles.Clear();
        CharacterRoles.AddRange(newRoles);
    }

    private static void ValidateRequiredProperty(string value, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidPropertyException(propertyName);
        }
    }

    private static void ValidateLifeSpan(DateTime dateOfBirth, DateTime dateOfDeath)
    {
        if (dateOfBirth > dateOfDeath)
        {
            throw new InvalidLifeSpanException(dateOfBirth, dateOfDeath);
        }
    }
}