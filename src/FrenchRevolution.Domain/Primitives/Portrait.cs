using FrenchRevolution.Domain.Exceptions;

namespace FrenchRevolution.Domain.Primitives;

public sealed class Portrait : ValueObject
{
    public string Url { get; }
    
    public Portrait(string url)
    {
        if (string.IsNullOrWhiteSpace(url)
            // TODO: Use regex instead
            // || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
            )
        {
            throw new InvalidPortraitException(url);
        }

        Url = url;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Url;
    }
}