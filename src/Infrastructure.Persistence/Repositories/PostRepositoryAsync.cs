using System;
using System.Threading.Tasks;
using Core.Domain.Persistence.Entities;
using Core.Domain.Persistence.Interfaces;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
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
