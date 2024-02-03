using CleanArchitecture.Domain.Contracts;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace CleanArchitecture.Infrastructure
{
    public class Repository<T, TId> : IRepository<T, TId> where T : class , IEntity<TId>
    {
        protected readonly ApplicationDbContext _dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> Entities => _dbContext.Set<T>();

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);

            return entity;
        }

        public async Task AddRangeAsync(IList<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
        }

        public async Task UpdateRangeAsync(IList<T> entities)
        {
            foreach (var entity in Entities)
            {
                await this.UpdateAsync(entity);
            }
        }

        public Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);

            return Task.CompletedTask;
        }

        public Task DeleteRangeAsync(IList<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);

            return Task.CompletedTask;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext
                .Set<T>()
                .ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbContext
                .Set<T>()
                .Where(filter)
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(TId id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            return await _dbContext
                .Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task UpdateAsync(T entity)
        {
            T exist = _dbContext.Set<T>().Find(entity.Id) 
                ?? throw new ArgumentNullException("_dbContext.Set<T>().Find(entity.Id)");

            _dbContext.Entry(exist).CurrentValues.SetValues(entity);

            return Task.CompletedTask;
        }
    }
}
