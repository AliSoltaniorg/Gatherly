using Gatherly.Domain.Enumerations;
using Gatherly.Domain.Primitives;

namespace Gatherly.Domain.Entities
{
  public class Invitation : Entity<Guid>
  {
    internal Invitation(
      Guid id,
      Guid memberId,
      Guid gatheringId
    ) : base(id)
    {
      MemberId = memberId;
      GatheringId = gatheringId;
      Status = InvitationStatus.Pending;
      CreatedOn = DateTime.UtcNow;
    }

    public Guid MemberId { get; private set; }
    public Guid GatheringId { get; private set; }
    public InvitationStatus Status { get; internal set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime? ModifiedOn { get; private set; }

    public bool IsPending() => Status == InvitationStatus.Pending;

    internal void Expire()
    {
      Status = InvitationStatus.Expired;
      ModifiedOn = DateTime.UtcNow;
    }

    internal Attendee Accept()
    {
      Status = InvitationStatus.Expired;
      ModifiedOn = DateTime.UtcNow;

      return new Attendee(this);
    }
  }
}
