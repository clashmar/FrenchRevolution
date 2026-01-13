using System.ComponentModel.DataAnnotations;
using FrenchRevolution.Domain.Exceptions;

namespace FrenchRevolution.Domain.Entities;

public class Character
{
    [Key] public Guid Id { get; private set; }
    [MaxLength(100)] public string Name { get; private set; } = string.Empty;
    [MaxLength(100)] public string Profession { get; private set; } = string.Empty;
    public DateTime DateOfBirth { get; private set; }
    public DateTime DateOfDeath { get; private set; }

    private Character() {}
    
    public Character(
        string name, 
        string profession,
        DateTime dateOfBirth,
        DateTime dateOfDeath
        )
    {
        ValidateRequiredProperty(name, nameof(name));
        ValidateRequiredProperty(profession, nameof(profession));
        ValidateLifeSpan(dateOfBirth, dateOfDeath);
        
        Id = Guid.NewGuid();
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