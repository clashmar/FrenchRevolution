using System.ComponentModel.DataAnnotations;

namespace FrenchRevolution.Domain.Entities;

public class Character
{
    [Key]
    public Guid Id { get; private set; }
    
    [MaxLength(100)]
    public string Name { get; private set; } = string.Empty;
    
    [MaxLength(100)]
    public string Profession { get; private set; } = string.Empty;
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
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        ArgumentException.ThrowIfNullOrEmpty(profession, nameof(profession));
        
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
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        ArgumentException.ThrowIfNullOrEmpty(profession, nameof(profession));

        if (dateOfDeath < dateOfBirth)
        {
            throw new InvalidOperationException("Date of death cannot be earlier than date of birth.");
        }
        
        Name = name;
        Profession = profession;
        DateOfBirth = DateTime.SpecifyKind(dateOfBirth, DateTimeKind.Utc);
        DateOfDeath = DateTime.SpecifyKind(dateOfDeath, DateTimeKind.Utc);
    }
}