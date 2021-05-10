using Core.Application.Contracts.Response;
using MediatR;

namespace Core.Application.Contracts.HandlerExchanges.Category.Queries
{
    public class GetAllCategoryQuery : IRequest<Response<PaginatedList<GetAllCategoryQueryResponse>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
