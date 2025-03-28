namespace Tracklet.Infra.Data.Repositories.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}