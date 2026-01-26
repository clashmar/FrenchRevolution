using FrenchRevolution.Domain.Primitives;

namespace FrenchRevolution.Domain.Data;

public sealed class CharacterFaction : Entity
{
    public Guid CharacterId { get; private set; }
    public Guid FactionId { get; private set; }
    public Character Character { get; private set; } = null!;
    public Faction Faction { get; private set; } = null!;

    public CharacterFaction(Guid characterId, Guid factionId) : base(Guid.NewGuid())
    {
        CharacterId = characterId;
        FactionId = factionId;
    }

    private CharacterFaction() : base(Guid.Empty) { }
}
