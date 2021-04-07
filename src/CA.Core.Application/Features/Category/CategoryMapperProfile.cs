using AutoMapper;
using CA.Core.Application.Contracts.Features.Category.Commands;
using CA.Core.Application.Contracts.Features.Category.Queries;

namespace CA.Core.Application.Features.Category
{
    public class CategoryMapperProfile : Profile
    {
        public CategoryMapperProfile()
        {
            CreateMap<AddCategoryCommand, Domain.Persistence.Entities.Category>();
            CreateMap<Domain.Persistence.Entities.Category, GetAllCategoryQueryViewModel>();
        }
    }
}
