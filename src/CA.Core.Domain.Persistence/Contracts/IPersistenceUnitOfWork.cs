using System;
using System.Threading.Tasks;
using CA.Core.Domain.Persistence.Entities;
using LinqToDB.Data;

namespace CA.Core.Domain.Persistence.Contracts
{
    public interface IPersistenceUnitOfWork : IDisposable
    {
        public IRepositoryAsync<Post> Post { get; }
        public IRepositoryAsync<Comment> Comment { get; }
        /// <summary>
        ///     Linq2Db instance of current database. Use it for bulk insert and bulk fetch. 
        /// </summary>
        DataConnection Linq2Db { get; }
        Task<int> CommitAsync();
    }
}
