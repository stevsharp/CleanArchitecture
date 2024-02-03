

namespace CleanArchitecture.Infrastructure
{
    public interface IUnitOfWork<TId> : IDisposable
    {
        Task<int> Commit(CancellationToken cancellationToken);

        Task Rollback();
    }
}
