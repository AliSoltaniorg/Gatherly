using Gatherly.Domain.Enumerations;
using Gatherly.Domain.Primitives;

namespace Gatherly.Domain.Entities
{
  public sealed class Gathering : Entity<Guid>
  {
    private Gathering(
      Guid id,
      Member? creator,
      GatheringType type,
      DateTime scheduledAt,
      string name,
      string? location
    ):base(id)
    {
      Creator = creator;
      Type = type;
      ScheduledAt = scheduledAt;
      Name = name;
      Location = location;
    }

    public Member? Creator { get; private set; }
    public GatheringType Type { get; private set; }
    public DateTime ScheduledAt { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Location { get; private set; }
    public DateTime? InvitationsExpireAt { get; private set; }
    public int? MaximumNumberOfAttendees { get; private set; }
    public int NumberOfAttendees { get; private set; }
    private List<Invitation> _invitations => new List<Invitation>();
    public IReadOnlyCollection<Invitation> Invitations => _invitations;
    public List<Attendee> _attendees => new List<Attendee>();
    public IReadOnlyCollection<Attendee> Attendees => _attendees;

    public static Gathering Create(
      Member? creator,
      GatheringType type,
      DateTime scheduledAt,
      string name,
      string? location,
      int? maximumNumberOfAttendees,
      int? invitationsValidBeforeInHours
    )
    {
      var gathering = new Gathering(
        Guid.NewGuid(),
        creator,
        type,
        scheduledAt,
        name,
        location
      );

      switch (gathering.Type)
      {
        case GatheringType.WithFixedNumberOfAttendees:
          if (!maximumNumberOfAttendees.HasValue)
            throw new Exception($"{nameof(MaximumNumberOfAttendees)} can't be null.");
          gathering.MaximumNumberOfAttendees = maximumNumberOfAttendees;
          break;
        case GatheringType.WithExpirationForInvitations:
          if (!invitationsValidBeforeInHours.HasValue)
            throw new Exception($"{nameof(InvitationsExpireAt)} can't be null.");
          gathering.InvitationsExpireAt = gathering.ScheduledAt.AddHours(-invitationsValidBeforeInHours.Value);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(Type));
      }

      return gathering;
    }

    public void Verify(Guid memberId)
    {
      if (Creator?.Id == memberId)
        throw new Exception("Can't send invitation to the gathering creator.");
      if (ScheduledAt < DateTime.UtcNow)
        throw new Exception("Can't send invitation for gathering in the past.");
    }

    public Invitation GetInvitation(Guid memberId)
    {
      Invitation invitation = new Invitation(
        Guid.NewGuid(),
        memberId,
        Id
      );

      _invitations.Add(invitation);

      return invitation;
    }

    private bool Expired()
      => (Type == GatheringType.WithFixedNumberOfAttendees &&
        NumberOfAttendees == MaximumNumberOfAttendees) ||
        (Type == GatheringType.WithExpirationForInvitations &&
        InvitationsExpireAt < DateTime.UtcNow);


    public Attendee? AcceptInvitation(Invitation invitation)
    {
      if(Expired())
      {
        invitation.Expire();
        return null;
      }
      Attendee attendee = invitation.Accept();
      _attendees.Add(attendee);
      NumberOfAttendees++;
      return attendee;
    }
  }
}
