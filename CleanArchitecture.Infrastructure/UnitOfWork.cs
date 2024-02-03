
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure
{

    public interface IUnitOfWork<TId> : IDisposable
    {
        Task<int> Commit(CancellationToken cancellationToken);

        Task Rollback();
    }
    public class UnitOfWork<TId> : IUnitOfWork<TId>
    {

        protected readonly ApplicationDbContext _dbContext;
        private bool disposed;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Commit(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task Rollback()
        {
            _dbContext
                .ChangeTracker
                .Entries()
                .ToList()
                .ForEach(x => x.Reload());

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose managed resources
                    _dbContext.Dispose();
                }
            }
            //dispose unmanaged resources
            disposed = true;
        }
    }
}
