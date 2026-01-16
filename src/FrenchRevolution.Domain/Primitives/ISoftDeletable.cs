namespace FrenchRevolution.Domain.Primitives;

public interface ISoftDeletable
{
    bool IsDeleted { get; }
    
    DateTime? DeletedAt { get; }
}