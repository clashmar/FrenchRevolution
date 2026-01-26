namespace FrenchRevolution.Domain.Primitives;

public abstract class Entity : IEquatable<Entity>, ISoftDeletable
{
    protected Entity() { }
    protected Entity(Guid id) => Id = id;
    public Guid Id { get; private init; }
    public bool IsDeleted { get; set; }
    
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }

    public static bool operator ==(Entity? first, Entity? second)
    {
        return first is not null && second is not null && first.Equals(second);
    }

    public static bool operator !=(Entity? first, Entity second)
    {
        return !(first == second);
    }

    public bool Equals(Entity? other)
    {
        if (other is null)
        {
            return false;
        }

        if (other.GetType() != GetType())
        {
            return false;
        }
        
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        if (obj is not Entity entity)
        {
            return false;  
        }
        
        return Id == entity.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode() * 43;
    }
}  