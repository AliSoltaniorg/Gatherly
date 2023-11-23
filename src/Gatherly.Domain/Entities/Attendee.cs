namespace Gatherly.Domain.Entities
{
  public class Attendee
  {
    internal Attendee(Invitation invitation)
    {
      MemberId = invitation.MemberId;
      GatheringId = invitation.GatheringId;
      CreatedOn = DateTime.UtcNow;
    }

    public Guid MemberId { get; private set; }
    public Guid GatheringId { get; private set; }
    public DateTime CreatedOn { get; private set; }

  }
}
