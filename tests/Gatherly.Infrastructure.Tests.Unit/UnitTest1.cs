using Gatherly.Infrastructure.Services.Reporting;

namespace Gatherly.Infrastructure.Tests.Unit
{
  public class UnitTest1
  {
    [Fact]
    public void Test1()
    {
      var d = new ExcelHelperService();
      ExcelBuilder s = new ExcelBuilder();
      //d.Build(null);
    }
  }
}