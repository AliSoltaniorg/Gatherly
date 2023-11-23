using Gatherly.Application.Gatherings.Commands.CreateGathering;
using Gatherly.Domain.Enumerations;

namespace Gatherly.Application.Tests.Unit.Gatherings
{
  public class CreateGatheringTests
  {
    private readonly Mock<IMemberRepository> _memberRepositoryMock;
    private readonly Mock<IGatheringRepository> _gatheringRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    private readonly CreateGatheringCommandHandler _gatheringCommandHandler;

    public CreateGatheringTests()
    {
      _memberRepositoryMock = new Mock<IMemberRepository>();
      _gatheringRepositoryMock = new Mock<IGatheringRepository>();
      _unitOfWorkMock = new Mock<IUnitOfWork>();
      _gatheringCommandHandler = new CreateGatheringCommandHandler(
        _memberRepositoryMock.Object,
        _gatheringRepositoryMock.Object,
        _unitOfWorkMock.Object
      );
    }

    [Fact]
    public async Task ShouldDoNothingWhenMemberIsNull()
    {
      //arrange
      CreateGatheringCommand gatheringCommand =
        new CreateGatheringCommand(
          Guid.NewGuid(),
          GatheringType.WithFixedNumberOfAttendees,
          DateTime.UtcNow,
          "ASP.NET",
          "Canada",
          null,
          3
        );

      _memberRepositoryMock.Setup(ex => ex.GetByIdAsync(It.IsAny<Guid>(), default)).ReturnsAsync(() => null);

      //act
      Task result = _gatheringCommandHandler.Handle(gatheringCommand, default);

      //assert
      var exception = await Record.ExceptionAsync(async () => await result);
      Assert.Null(exception);
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenGatheringTypeIsWithExpirationForInvitationsAndInvitationsValidBeforeInHoursIsNull()
    {
      //arrange
      CreateGatheringCommand gatheringCommand =
        new CreateGatheringCommand(
          Guid.NewGuid(),
          GatheringType.WithExpirationForInvitations,
          DateTime.UtcNow,
          "ASP.NET",
          "Canada",
          2,
          null
        );

      _memberRepositoryMock.Setup(ex => ex.GetByIdAsync(It.IsAny<Guid>(), default))
        .ReturnsAsync(new Member()
        {

        });

      //act
      Task result = _gatheringCommandHandler.Handle(gatheringCommand, default);

      //assert
      var exception = await Assert.ThrowsAsync<Exception>(async () => await result);
      Assert.Contains(nameof(Gathering.InvitationsExpireAt), exception.Message);
    } 
    [Fact]
    public async Task ShouldThrowExceptionWhenGatheringTypeIsWithFixedNumberOfAttendeesAndMaximumNumberOfAttendeesIsNull()
    {
      //arrange
      CreateGatheringCommand gatheringCommand =
        new CreateGatheringCommand(
          Guid.NewGuid(),
          GatheringType.WithFixedNumberOfAttendees,
          DateTime.UtcNow,
          "ASP.NET",
          "Canada",
          null,
          3
        );

      _memberRepositoryMock.Setup(ex => ex.GetByIdAsync(It.IsAny<Guid>(), default))
        .ReturnsAsync(new Member()
        {

        });

      //act
      Task result = _gatheringCommandHandler.Handle(gatheringCommand, default);

      //assert
      var exception = await Assert.ThrowsAsync<Exception>(async () => await result);
      Assert.Contains(nameof(Gathering.MaximumNumberOfAttendees), exception.Message);
    }

    [Fact]
    public async Task ShouldCallAddRepositoryAndSaveUnitOfWork()
    {
      //arrange
      CreateGatheringCommand gatheringCommand =
        new CreateGatheringCommand(
          Guid.NewGuid(),
          GatheringType.WithFixedNumberOfAttendees,
          DateTime.UtcNow,
          "ASP.NET",
          "Canada",
          1,
          3
        );

      _memberRepositoryMock.Setup(ex => ex.GetByIdAsync(It.IsAny<Guid>(), default)).ReturnsAsync(new Member { });

      //act
      await _gatheringCommandHandler.Handle(gatheringCommand, default);

      //assert
      _gatheringRepositoryMock.Verify(ex => ex.Add(It.IsAny<Gathering>()), Times.Once);
      _unitOfWorkMock.Verify(ex => ex.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
  }
}
