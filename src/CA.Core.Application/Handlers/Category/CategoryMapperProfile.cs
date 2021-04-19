using AutoMapper;
using CA.Core.Application.Contracts.HandlerExchanges.Category.Commands;
using CA.Core.Application.Contracts.HandlerExchanges.Category.Queries;

namespace CA.Core.Application.Handlers.Category
{
    public class CategoryMapperProfile : Profile
    {
        public CategoryMapperProfile()
        {
            CreateMap<AddCategoryCommand, Domain.Persistence.Entities.Category>();
            CreateMap<Domain.Persistence.Entities.Category, GetAllCategoryQueryResponse>();
        }
    }
}
