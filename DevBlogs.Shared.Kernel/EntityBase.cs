using System.ComponentModel.DataAnnotations.Schema;

namespace DevBlogs.Shared.Kernel;

public abstract class EntityBase
{
    private int? _requestedHashCode;
    private int _Id;

    public virtual int Id
    {
        get
        {
            return _Id;
        }
        protected set
        {
            _Id = value;
        }
    }

    #region Domain Events
    private List<DomainEventBase> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<DomainEventBase> DomainEvents
        => _domainEvents.AsReadOnly();

    protected void RegisterDomainEvent(DomainEventBase domainEvent)
        => _domainEvents.Add(domainEvent);

    public void RemoveDomainEvent(DomainEventBase domainEvent)
        => _domainEvents?.Remove(domainEvent);

    public void ClearDomainEvents()
        => _domainEvents.Clear();
    #endregion

    public bool IsTransient()
    {
        // this.Id
        return Id == default;
    }

    public override int GetHashCode()
    {
        if (!IsTransient())
        {
            if (!_requestedHashCode.HasValue)
            // this.Id
            { _requestedHashCode = Id.GetHashCode() ^ 31; }

            return _requestedHashCode.Value;
        }

        return base.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is EntityBase))
        { return false; }

        if (Object.ReferenceEquals(this, obj))
        { return true; }

        if (GetType() != obj.GetType())
        { return false; }

        EntityBase item = (EntityBase)obj;

        // this.IsTransient()
        if (item.IsTransient() || IsTransient())
        { return false; }

        // this.Id
        return item.Id == Id;
    }

    public static bool operator ==(EntityBase left, EntityBase right)
    {
        if (Equals(left, null))
        {
            return Equals(right, null) ? true : false;
        }

        return left.Equals(right);
    }

    public static bool operator !=(EntityBase left, EntityBase right)
    {
        return !(left == right);
    }
 
}
