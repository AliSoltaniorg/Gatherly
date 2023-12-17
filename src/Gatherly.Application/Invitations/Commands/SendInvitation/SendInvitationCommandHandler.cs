using Gatherly.Application.Abstractions.Email;
using Gatherly.Domain.Entities;
using Gatherly.Domain.Enumerations;
using Gatherly.Domain.Repositories;
using MediatR;

namespace Gatherly.Application.Invitations.Commands.SendInvitation
{
    internal sealed class SendInvitationCommandHandler : IRequestHandler<SendInvitationCommand, Unit>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IGatheringRepository _gatheringRepository;
        private readonly IInvitationRepository _invitationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public SendInvitationCommandHandler(
          IMemberRepository memberRepository,
          IGatheringRepository gatheringRepository,
          IInvitationRepository invitationRepository,
          IUnitOfWork unitOfWork,
          IEmailService emailService
        )
        {
            _memberRepository = memberRepository;
            _gatheringRepository = gatheringRepository;
            _invitationRepository = invitationRepository;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<Unit> Handle(SendInvitationCommand request, CancellationToken cancellationToken)
        {
            Member member = await _memberRepository.GetByIdAsync(request.MemberId, cancellationToken);

            Gathering gathering = await _gatheringRepository.GetByIdWithCreatorAsync(request.GatheringId, cancellationToken);

            if (member is null || gathering is null)
                return Unit.Value;

            gathering.Verify(member.Id);

            Invitation invitation = gathering.GetInvitation(member.Id);

            _invitationRepository.Add(invitation);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _emailService.SendInvitationSendEmailAsync(member, gathering);

            return Unit.Value;
        }
    }
}
