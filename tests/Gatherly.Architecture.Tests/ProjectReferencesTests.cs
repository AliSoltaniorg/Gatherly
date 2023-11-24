namespace Gatherly.Architecture.Tests
{
  public class ProjectReferencesTests
  {
    private const string DomainAssemblyName = "Gatherly.Domain";
    private const string ApplicationAssemblyName = "Gatherly.Application";
    private const string InfrastructureAssemblyName = "Gatherly.Infrastructure";

    [Fact]
    public void Domain_Should_NotHaveDependencies()
    {
      //arrange
      var excludedProjectAssemblies = new[] { DomainAssemblyName,ApplicationAssemblyName,InfrastructureAssemblyName };

      //act
      var references = VisualStudioProvider.GetReferences(DomainAssemblyName);

      //assert
      Assert.True(references.Intersect(excludedProjectAssemblies).Count() == 0);
    }

    [Fact]
    public void Application_Should_HaveDomainAndInfrastructureDependencies()
    {
      //arrange
      var excludedProjectAssemblies = new[] { DomainAssemblyName, InfrastructureAssemblyName };

      //act
      var references = VisualStudioProvider.GetReferences(ApplicationAssemblyName);

      //assert
      Assert.True(references.SequenceEqual(excludedProjectAssemblies));
    }

    [Fact]
    public void Infrastructure_Should_HaveDomainReference()
    {
      //arrange
      var excludedProjectAssemblies = new[] { DomainAssemblyName };

      //act
      var references = VisualStudioProvider.GetReferences(InfrastructureAssemblyName);

      //assert
      Assert.True(references.SequenceEqual(excludedProjectAssemblies));
    }
  }
}