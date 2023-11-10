using Gatherly.Domain.Enumerations;
using MediatR;

namespace Gatherly.Application.Gatherings.Commands
{
  public sealed record CreateGatheringCommand(
    Guid MemberId,
    GatheringType Type,
    DateTime ScheduledAtUtc,
    string Name,
    string? Location,
    int? MaximumNumberOfAttendees,
    int? InvitationsValidBeforeInHours
  ) : IRequest;
}
