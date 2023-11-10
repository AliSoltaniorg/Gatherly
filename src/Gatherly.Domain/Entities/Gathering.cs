using Gatherly.Domain.Enumerations;

namespace Gatherly.Domain.Entities
{
  public class Gathering
  {
    private Gathering(
      Guid id,
      Member? creator,
      GatheringType type,
      DateTime scheduledAt,
      string name,
      string? location
    )
    {
      Id = id;
      Creator = creator;
      Type = type;
      ScheduledAt = scheduledAt;
      Name = name;
      Location = location;
    }

    public Guid Id { get; private set; }
    public Member? Creator { get; private set; } 
    public GatheringType Type { get; private set; }
    public DateTime ScheduledAt { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Location { get; private set; }
    public DateTime? InvitationsExpireAt { get; private set; }
    public int? MaximumNumberOfAttendees { get; private set; }
    public int NumberOfAttendees { get; private set; }
    public ICollection<Invitation>? Invitations { get; private set; }
    public ICollection<Attendee>? Attendees { get; private set; }

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
            throw new Exception($"{nameof(maximumNumberOfAttendees)} can't be null.");
          gathering.MaximumNumberOfAttendees = maximumNumberOfAttendees;
          break;
        case GatheringType.WithExpirationForInvitations:
          if (!invitationsValidBeforeInHours.HasValue)
            throw new Exception($"{nameof(invitationsValidBeforeInHours)} can't be null.");
          gathering.InvitationsExpireAt = gathering.ScheduledAt.AddHours(-invitationsValidBeforeInHours.Value);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(Type));
      }

      return gathering;
    }
  }
}
