using System;
using System.Threading.Tasks;
using CA.Core.Domain.Persistence.Contracts;
using CA.Core.Domain.Persistence.Entities;
using CA.Infrastructure.Persistence.Context;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;

namespace CA.Infrastructure.Persistence.Repositories
{
    public class PersistenceUnitOfWork : IPersistenceUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private bool _disposed;
        public IRepositoryAsync<Post> Post { get; }
        public IRepositoryAsync<Comment> Comment { get; }
        public DataConnection Linq2Db { get; }
        public PersistenceUnitOfWork(AppDbContext appDbContext, 
                                        IRepositoryAsync<Post> post, 
                                        IRepositoryAsync<Comment> comment)
        {
            _dbContext = appDbContext;
            Linq2Db = _dbContext.CreateLinqToDbConnection();
            Post = post;
            Comment = comment;

        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing) _dbContext.Dispose();
            _disposed = true;
        }
    }
}
