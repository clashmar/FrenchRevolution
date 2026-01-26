using FrenchRevolution.Domain.Exceptions;
using FrenchRevolution.Domain.Primitives;

namespace FrenchRevolution.Domain.Data;

public sealed class Faction : Entity
{
    public string Title { get; private set; }
    public string NormalizedTitle { get; private set; }
    public string Description { get; private set; }

    public IReadOnlyCollection<CharacterFaction> CharacterFactions { get; private set; } = new List<CharacterFaction>();

    public Faction(string title, string description) : base(Guid.NewGuid())
    {
        ValidateTitle(title);
        Title = title;
        NormalizedTitle = title.Trim().ToLowerInvariant();
        Description = description;
    }

    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new InvalidPropertyException(nameof(Title));
        }
    }
}
