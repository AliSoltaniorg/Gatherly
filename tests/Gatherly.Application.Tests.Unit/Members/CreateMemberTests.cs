using Gatherly.Application.Members.CreateMember;

namespace Gatherly.Application.Tests.Unit.Members
{
  public class CreateMemberTests
  {
    private readonly Mock<IMemberRepository> _memberRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    public CreateMemberTests()
    {
      _memberRepositoryMock = new Mock<IMemberRepository>();
      _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async void Handle_Should_CallAddMemberAndSave_ForCreateMember()
    {
      //arrange
      var memberCommand = new CreateMemberCommand("Ali","Soltani","AliSoltani@Gmail.com");

      var memberCommandHandler = new CreateMemberCommandHandler(_memberRepositoryMock.Object,_unitOfWorkMock.Object);

      //act
      await memberCommandHandler.Handle(memberCommand, default);

      //assert
      _memberRepositoryMock.Verify(ex => ex.Add(It.IsAny<Member>()));
      _unitOfWorkMock.Verify(ex => ex.SaveChangesAsync(It.IsAny<CancellationToken>()));
    }
  }
}
