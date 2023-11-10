using Gatherly.Domain.Enumerations;

namespace Gatherly.Domain.Entities
{
  public class Invitation
  {
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int GatheringId { get; set; }
    public InvitationStatus Status { get; set; }
  }
}
