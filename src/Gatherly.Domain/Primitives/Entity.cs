namespace Gatherly.Domain.Primitives;

public abstract class Entity<TKey> 
  where TKey : struct, IEquatable<TKey>
{
  protected Entity(TKey id)
  {
    Id = id;
  }
  public TKey Id { get; private init; }

  public static bool operator == (Entity<TKey>? first, Entity<TKey> second)
  {
    return first is not null && first.Equals(second);
  }

  public static bool operator !=(Entity<TKey>? first, Entity<TKey> second)
  {
    return !(first == second);
  }


  public bool Equals(TKey? other)
  {
    if (other is null)
      return false;

    if (other.GetType() != Id.GetType())
      return false;

    return (object)other == (object)Id;
  }

  public override bool Equals(object? obj)
  {
    if(obj is null)
      return false;

    if(obj.GetType() != GetType())
      return false;

    if(obj is not Entity<TKey> entity)
      return false;

    if (entity.Id.GetType() != Id.GetType())
      return false;

    return (object)entity.Id == (object)Id;
  }

  public override int GetHashCode()
  {
    return Id.GetHashCode() * 13;
  }
}

