using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Persistence.Interfaces;
using Infrastructure.Persistence.Context;
using LinqToDB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class RepositoryAsync<T> : IRepositoryAsync<T> where T : class
    {
        private readonly AppDbContext _dbContext;

        public RepositoryAsync(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }
        public virtual IQueryable<T> Entity => _dbContext.Set<T>();
        public virtual ITable<T> Table => _dbContext.Linq2Db.GetTable<T>();

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public virtual async Task<int> CountTotalAsync()
        {
            return await EntityFrameworkQueryableExtensions.CountAsync(_dbContext.Set<T>());
        }

        public virtual async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await EntityFrameworkQueryableExtensions.ToListAsync(_dbContext
                    .Set<T>());
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).CurrentValues.SetValues(entity);
            await Task.CompletedTask;
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await Task.CompletedTask;
        }
    }
}
