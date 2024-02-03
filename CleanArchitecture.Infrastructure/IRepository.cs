using CleanArchitecture.Domain.Contracts;

using System.Linq.Expressions;

namespace CleanArchitecture.Infrastructure
{
    public interface IRepository<T, in TId> where T : class , IEntity<TId>
    {
        IQueryable<T> Entities { get; }

        Task<T> GetByIdAsync(TId id);

        Task<List<T>> GetAllAsync();

        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter);

        Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize);

        Task<T> AddAsync(T entity);

        Task AddRangeAsync(IList<T> entities);

        Task UpdateAsync(T entity);

        Task UpdateRangeAsync(IList<T> entities);

        Task DeleteAsync(T entity);

        Task DeleteRangeAsync(IList<T> entities);
    }
}
