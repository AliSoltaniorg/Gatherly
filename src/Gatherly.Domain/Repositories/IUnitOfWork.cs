namespace Gatherly.Domain.Repositories
{
  public interface IUnitOfWork
  {
    Task<int> SaveChangesAsync(CancellationToken? cancellationToken = null);
  }
}
