using AutoMapper;
using CA.Core.Application.Contracts.Features.Category.Commands;

namespace CA.Core.Application.Features.Category
{
    public class CategoryMapperProfile : Profile
    {
        public CategoryMapperProfile()
        {
            CreateMap<AddCategoryCommand, Domain.Persistence.Entities.Category>();
        }
    }
}
