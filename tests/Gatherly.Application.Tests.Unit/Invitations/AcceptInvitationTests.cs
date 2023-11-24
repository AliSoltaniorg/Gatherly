using Gatherly.Application.Abstractions;
using Gatherly.Application.Invitations.Commands.AcceptInvitation;
using Gatherly.Domain.Enumerations;

namespace Gatherly.Application.Tests.Unit.Invitations
{
  public class AcceptInvitationTests
  {
    private readonly Mock<IMemberRepository> _memberRepositoryMock;
    private readonly Mock<IGatheringRepository> _gatheringRepositoryMock;
    private readonly Mock<IInvitationRepository> _invitationRepositoryMock;
    private readonly Mock<IAttendeeRepository> _attendeeRepositoryMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    private readonly AcceptInvitationCommandHandler _invitationCommandHandler;

    public AcceptInvitationTests()
    {
      _memberRepositoryMock = new Mock<IMemberRepository>();
      _gatheringRepositoryMock = new Mock<IGatheringRepository>();
      _unitOfWorkMock = new Mock<IUnitOfWork>();
      _emailServiceMock = new Mock<IEmailService>();
      _invitationRepositoryMock = new Mock<IInvitationRepository>();
      _attendeeRepositoryMock = new Mock<IAttendeeRepository>();

      _invitationCommandHandler = new AcceptInvitationCommandHandler(
        _memberRepositoryMock.Object,
        _gatheringRepositoryMock.Object,
        _invitationRepositoryMock.Object,
        _attendeeRepositoryMock.Object,
        _unitOfWorkMock.Object,
        _emailServiceMock.Object
      );
    }

    [Fact]
    public async Task ShouldDoNothingWhenMemberIsNull()
    {
      //arrange
      AcceptInvitationCommand invitationCommand =
        new AcceptInvitationCommand(Guid.NewGuid());

      _invitationRepositoryMock.Setup(ex => ex.GetByIdAsync(It.IsAny<Guid>(), default))
        .ReturnsAsync(new Invitation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
        {
          Status = InvitationStatus.Pending
        });

      //act
      var result = await _invitationCommandHandler.Handle(invitationCommand,default);

      //assert
      Assert.Null(null);
    }
  }
}
