namespace ExemploGRPC.Domain.Entities.Base;

/// <summary>
/// Base class for all entities in the domain
/// Implements DDD Entity pattern
/// </summary>
public abstract class EntityBase
{
    /// <summary>
    /// Unique identifier for the entity
    /// </summary>
    public Guid Id { get; protected set; }

    /// <summary>
    /// Date and time when the entity was created
    /// </summary>
    public DateTime CreatedAt { get; protected set; }

    /// <summary>
    /// Date and time when the entity was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; protected set; }

    /// <summary>
    /// Indicates if the entity has been logically deleted
    /// </summary>
    public bool IsDeleted { get; protected set; }

    protected EntityBase()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    /// <summary>
    /// Marks the entity as updated
    /// </summary>
    public virtual void MarkAsUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the entity as deleted (soft delete)
    /// </summary>
    public virtual void MarkAsDeleted()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Validates the entity
    /// </summary>
    public abstract void Validate();

    public override bool Equals(object? obj)
    {
        if (obj is not EntityBase other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(EntityBase? a, EntityBase? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(EntityBase? a, EntityBase? b)
    {
        return !(a == b);
    }
}
