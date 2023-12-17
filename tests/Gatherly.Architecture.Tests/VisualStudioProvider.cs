using System.Xml.Linq;

namespace Gatherly.Architecture.Tests
{
  public static class VisualStudioProvider
  {
    public static string TryGetSolutionPath(string? currentPath = null)
    {
      var directory = new DirectoryInfo(
          currentPath ?? Directory.GetCurrentDirectory());
      while (directory != null && !directory.GetFiles("*.sln").Any())
      {
        directory = directory.Parent;
      }
      return directory?.FullName ?? "";
    }

    public static string TryGetProjectPathByName(string assemblyName)
    {
      var solutionPath = TryGetSolutionPath();
      var csProjectFilePaths = Directory.GetFiles(solutionPath, "*.csproj", SearchOption.AllDirectories);
      return csProjectFilePaths.SingleOrDefault(p => Path.GetFileName(p).Replace(".csproj","") == assemblyName) ?? "";
    }

    public static IEnumerable<string?> GetReferences(string assemblyName)
    {
      return XDocument.Load(TryGetProjectPathByName(assemblyName))
        .Descendants("ProjectReference")?
        .Select(c => c.Attribute("Include")?.Value)
        .Select(Path.GetFileName)
        .Select(x => x?.Replace(".csproj", "")) ?? Enumerable.Empty<string>();
    }
  }
}