using Gatherly.Domain.Primitives;

namespace Gatherly.Domain.Entities
{
  public sealed class Member : Entity<Guid>
  {
    public Member(
      Guid id,
      string firstName,
      string lastName,
      string email
    ):base(id)
    {
      FirstName = firstName;
      LastName = lastName;
      Email = email;
    }

    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public ICollection<Invitation>? Invitations { get; set; }

  }
}
