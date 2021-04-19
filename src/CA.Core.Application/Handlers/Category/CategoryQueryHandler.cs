using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CA.Core.Application.Contracts.HandlerExchanges.Category.Queries;
using CA.Core.Application.Contracts.Response;
using CA.Core.Domain.Persistence.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CA.Core.Application.Handlers.Category
{
    public class CategoryQueryHandler : IRequestHandler<GetAllCategoryQuery, PaginatedList<GetAllCategoryQueryResponse>>
    {
        private readonly IPersistenceUnitOfWork _persistenceUnitOfWorkpe;
        private readonly IMapper _mapper;

        public CategoryQueryHandler(IMapper mapper, IPersistenceUnitOfWork persistenceUnitOfWorkpe)
        {
            _mapper = mapper;
            _persistenceUnitOfWorkpe = persistenceUnitOfWorkpe;
        }

        public async Task<PaginatedList<GetAllCategoryQueryResponse>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<Domain.Persistence.Entities.Category, GetAllCategoryQueryResponse>());
            var cats =
                _persistenceUnitOfWorkpe.Category.Entity.ProjectTo<GetAllCategoryQueryResponse>(configuration);

            return await PaginatedList<GetAllCategoryQueryResponse>.CreateAsync(cats.AsNoTracking(),
                request.PageNumber ?? 1, request.PageSize ?? 12);
        }
    }
}
