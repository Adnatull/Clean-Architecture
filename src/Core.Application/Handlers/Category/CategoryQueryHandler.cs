using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Application.Contracts.HandlerExchanges.Category.Queries;
using Core.Application.Contracts.Response;
using Core.Domain.Persistence.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Application.Handlers.Category
{
    public class CategoryQueryHandler : IRequestHandler<GetAllCategoryQuery, Response<PaginatedList<GetAllCategoryQueryResponse>>>
    {
        private readonly IPersistenceUnitOfWork _persistenceUnitOfWork;
        private readonly IMapper _mapper;

        public CategoryQueryHandler(IMapper mapper, IPersistenceUnitOfWork persistenceUnitOfWork)
        {
            _mapper = mapper;
            _persistenceUnitOfWork = persistenceUnitOfWork;
        }

        public async Task<Response<PaginatedList<GetAllCategoryQueryResponse>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<Domain.Persistence.Entities.Category, GetAllCategoryQueryResponse>());
            var cats =
                _persistenceUnitOfWork.Category.Entity.ProjectTo<GetAllCategoryQueryResponse>(configuration);
            var rs = await PaginatedList<GetAllCategoryQueryResponse>.CreateFromEfQueryableAsync(cats.AsNoTracking(),
                request.PageNumber ?? 1, request.PageSize ?? 12);
            return Response<PaginatedList<GetAllCategoryQueryResponse>>.Success(rs, "Succeeded");
        }
    }
}
