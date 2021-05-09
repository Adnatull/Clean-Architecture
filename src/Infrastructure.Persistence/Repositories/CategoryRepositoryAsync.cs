using System;
using System.Threading.Tasks;
using Core.Domain.Persistence.Entities;
using Core.Domain.Persistence.Interfaces;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class CategoryRepositoryAsync : RepositoryAsync<Category>, ICategoryRepositoryAsync
    {
        private readonly DbSet<Category> _category;
        public CategoryRepositoryAsync(AppDbContext appDbContext) : base(appDbContext)
        {
            _category = appDbContext.Set<Category>();
        }

        public override async Task<Category> AddAsync(Category category)
        {
            if (category.Slug.Contains(' '))
            {
                throw new Exception("Slug can not contain space character");
            }
            await base.AddAsync(category);
            return category;
        }

        public override async Task UpdateAsync(Category category)
        {
            if (category.Slug.Contains(' '))
            {
                throw new Exception("Slug can not contain space character");
            }
            await base.UpdateAsync(category);
            await Task.CompletedTask;
        }
    }
}
