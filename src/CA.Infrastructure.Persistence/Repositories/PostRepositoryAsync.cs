using CA.Core.Domain.Persistence.Contracts;
using CA.Core.Domain.Persistence.Entities;
using CA.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CA.Infrastructure.Persistence.Repositories
{
    public class PostRepositoryAsync : RepositoryAsync<Post>, IPostRepositoryAsync
    {
        private readonly DbSet<Post> _post;
        public PostRepositoryAsync(AppDbContext appDbContext) : base(appDbContext)
        {
            _post = appDbContext.Set<Post>();
        }

        public override async Task<Post> AddAsync(Post post)
        {
            if (post.Slug.Contains(' '))
            {
                throw new Exception("Slug can not contain space character");
            }
            await base.AddAsync(post);
            return post;
        }

        public override async Task UpdateAsync(Post post)
        {
            if (post.Slug.Contains(' '))
            {
                throw new Exception("Slug can not contain space character");
            }
            await base.UpdateAsync(post);
            await Task.CompletedTask;
        }
    }
}
