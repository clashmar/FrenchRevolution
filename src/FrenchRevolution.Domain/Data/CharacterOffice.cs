using FrenchRevolution.Domain.Exceptions;
using FrenchRevolution.Domain.Primitives;

namespace FrenchRevolution.Domain.Data;

public sealed class CharacterOffice : Entity
{
    public Guid CharacterId { get; private set; } 
    public Guid OfficeId { get; private set; }
    public Character Character { get; private set; } = null!;
    public Office Office { get; private set; } = null!;
    public DateTime From { get; private set; }
    public DateTime To { get; private set; }
    
    public CharacterOffice(
        Guid characterId, 
        Guid officeId,
        DateTime from,
        DateTime to
        ) : base(Guid.NewGuid())
    {
        if (from > to)
        {
            throw new InvalidTimeSpanException(from, to);
        }
        
        CharacterId = characterId;
        OfficeId = officeId;
        From = DateTime.SpecifyKind(from, DateTimeKind.Utc);
        To = DateTime.SpecifyKind(to, DateTimeKind.Utc);
    }
    
    private CharacterOffice() : base(Guid.Empty) { }
}