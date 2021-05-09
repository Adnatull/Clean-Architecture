using System;
using System.Threading.Tasks;
using Core.Domain.Persistence.Entities;
using LinqToDB.Data;

namespace Core.Domain.Persistence.Interfaces
{
    public interface IPersistenceUnitOfWork : IDisposable
    {
        
        public IPostRepositoryAsync Post { get; }
        public ICategoryRepositoryAsync Category { get; }
        public ITagRepositoryAsync Tag { get; }
        public IRepositoryAsync<Comment> Comment { get; }
        /// <summary>
        ///     Linq2Db instance of current database. Use it for bulk insert and bulk fetch. 
        /// </summary>
        DataConnection Linq2Db { get; }
        Task<int> CommitAsync();
    }
}
