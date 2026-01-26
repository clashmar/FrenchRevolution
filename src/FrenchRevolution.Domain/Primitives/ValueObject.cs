namespace FrenchRevolution.Domain.Primitives;

public abstract class ValueObject : IEquatable<ValueObject>
{
    public static bool operator ==(ValueObject first, ValueObject second)
    {
        if (ReferenceEquals(first, null) ^ ReferenceEquals(second, null))
        {
            return false;
        }
        return first is not null && (ReferenceEquals(first, second) || first.Equals(second));
    }

    public static bool operator !=(ValueObject? first, ValueObject? second)
    {
        return first is not null && second is not null && !(first == second);
    }

    protected abstract IEnumerable<object> GetEqualityComponents();

    public bool Equals(ValueObject? other) => Equals((object?)other);

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;

        return GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x.GetHashCode())
            .Aggregate((x, y) => x ^ y);
    }
}