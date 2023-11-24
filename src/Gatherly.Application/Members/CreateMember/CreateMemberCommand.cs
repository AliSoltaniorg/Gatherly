using Gatherly.Domain.Entities;
using Gatherly.Domain.Repositories;
using MediatR;

namespace Gatherly.Application.Members.CreateMember
{
  public sealed record CreateMemberCommand(
    string FirstName,
    string LastName,
    string Email): IRequest;

  public sealed class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand>
  {
    private readonly IMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateMemberCommandHandler(
      IMemberRepository memberRepository,
      IUnitOfWork unitOfWork
    )
    {
      _memberRepository = memberRepository;
      _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
      Member member = new Member(
        Guid.NewGuid(),
        request.FirstName,
        request.LastName,
        request.Email
      );

      _memberRepository.Add(member);

      await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
  }
}
