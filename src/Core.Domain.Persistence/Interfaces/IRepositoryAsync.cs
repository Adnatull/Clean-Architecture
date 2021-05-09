using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;

namespace Core.Domain.Persistence.Interfaces
{
    public interface IRepositoryAsync<T> where T : class
    {
        /// <summary>
        ///     Iqueryable entity of Entity Framework. Use this to execute query in database level.
        /// </summary>
        IQueryable<T> Entity { get; }

        /// <summary>
        ///     Linq2Db instance of T table. Use it for bulk insert and bulk fetch. 
        /// </summary>
        ITable<T> Table { get; }
        Task<T> GetByIdAsync(int id);

        Task<int> CountTotalAsync();

        Task<IReadOnlyList<T>> GetAllAsync();

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}
