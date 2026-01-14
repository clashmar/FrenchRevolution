using FrenchRevolution.Domain.Exceptions;
using FrenchRevolution.Domain.Primitives;

namespace FrenchRevolution.Domain.Data;

public sealed class Role : Entity
{
    public string Title { get; private set; }
    
    public string NormalizedTitle { get; private set; }

    public IReadOnlyCollection<CharacterRole> CharacterRoles { get; private set; } = new List<CharacterRole>();
    
    public Role(string title) : base(Guid.NewGuid())
    {
        ValidateTitle(title);
        Title = title;
        NormalizedTitle = title.Trim().ToLowerInvariant();
    }

    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new InvalidPropertyException(nameof(Title));
        }
    }
}