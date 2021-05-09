using System;
using System.Threading.Tasks;
using Core.Domain.Persistence.Entities;
using Core.Domain.Persistence.Interfaces;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class TagRepositoryAsync : RepositoryAsync<Tag>, ITagRepositoryAsync
    {
        private readonly DbSet<Tag> _tag;
        public TagRepositoryAsync(AppDbContext appDbContext) : base(appDbContext)
        {
            _tag = appDbContext.Set<Tag>();
        }

        public override async Task<Tag> AddAsync(Tag tag)
        {
            if (tag.Slug.Contains(' '))
            {
                throw new Exception("Slug can not contain space character");
            }
            await base.AddAsync(tag);
            return tag;
        }

        public override async Task UpdateAsync(Tag tag)
        {
            if (tag.Slug.Contains(' '))
            {
                throw new Exception("Slug can not contain space character");
            }
            await base.UpdateAsync(tag);
            await Task.CompletedTask;
        }
    }
}
