namespace FrenchRevolution.Domain.Primitives;

public abstract class Entity : ISoftDeletable
{
    public bool IsDeleted { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }
}