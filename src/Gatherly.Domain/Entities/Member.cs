namespace Gatherly.Domain.Entities
{
  public class Member
  {
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public ICollection<Invitation>? Invitations { get; set; }

  }
}
