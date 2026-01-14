using FrenchRevolution.Domain.Exceptions;
using FrenchRevolution.Domain.Primitives;

namespace FrenchRevolution.Domain.Data;

public sealed class CharacterRole : Entity
{
    public Guid CharacterId { get; private set; } 
    public Guid RoleId { get; private set; }
    public Character Character { get; private set; } = null!;
    public Role Role { get; private set; } = null!;
    public DateTime From { get; private set; }
    public DateTime To { get; private set; }
    
    public CharacterRole(
        Guid characterId, 
        Guid roleId,
        DateTime from,
        DateTime to
        ) : base(Guid.NewGuid())
    {
        if (from > to)
        {
            throw new InvalidTimeSpanException(from, to);
        }
        
        CharacterId = characterId;
        RoleId = roleId;
        From = DateTime.SpecifyKind(from, DateTimeKind.Utc);
        To = DateTime.SpecifyKind(to, DateTimeKind.Utc);
    }
    
    private CharacterRole() : base(Guid.Empty) { }
}