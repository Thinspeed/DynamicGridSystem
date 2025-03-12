namespace GridSystem.Domain.Abstractions;

public abstract class SoftDeletableEntity : Entity
{
    public bool IsDeleted { get; set; }
    
    public DateTime? DeletedAt { get; set; }
}