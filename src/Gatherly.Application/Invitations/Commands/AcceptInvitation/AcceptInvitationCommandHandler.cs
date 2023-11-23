using Gatherly.Application.Abstractions;
using Gatherly.Domain.Entities;
using Gatherly.Domain.Enumerations;
using Gatherly.Domain.Repositories;
using MediatR;

namespace Gatherly.Application.Invitations.Commands.AcceptInvitation
{
  internal sealed class AcceptInvitationCommandHandler : IRequestHandler<AcceptInvitationCommand, Unit>
  {
    private readonly IMemberRepository _memberRepository;
    private readonly IGatheringRepository _gatheringRepository;
    private readonly IInvitationRepository _invitationRepository;
    private readonly IAttendeeRepository _attendeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    private readonly IEmailService _emailService;

    public AcceptInvitationCommandHandler(
      IMemberRepository memberRepository,
      IGatheringRepository gatheringRepository,
      IInvitationRepository invitationRepository,
      IAttendeeRepository attendeeRepository,
      IUnitOfWork unitOfWork,
      IEmailService emailService
    )
    {
      _memberRepository = memberRepository;
      _gatheringRepository = gatheringRepository;
      _unitOfWork = unitOfWork;
      _invitationRepository = invitationRepository;
      _attendeeRepository = attendeeRepository;
      _emailService = emailService;
    }
    public async Task<Unit> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
    {
      Invitation invitation = await _invitationRepository
        .GetByIdAsync(request.InvitationId, cancellationToken);

      if (invitation is null || !invitation.IsPending())
        return Unit.Value;

      Member member = await _memberRepository.GetByIdAsync(invitation.MemberId, cancellationToken);

      Gathering gathering = await _gatheringRepository
        .GetByIdWithCreatorAsync(invitation.GatheringId, cancellationToken);

      if (member is null || gathering is null)
        return Unit.Value;

      var attendee = gathering.AcceptInvitation(invitation);

      if (attendee is not null)
      {
        _attendeeRepository.Add(attendee);
        await _emailService.SendInvitationSendEmailAsync(member, gathering);
      }

      await _unitOfWork.SaveChangesAsync(cancellationToken);


      return Unit.Value;
    }
  }
}
