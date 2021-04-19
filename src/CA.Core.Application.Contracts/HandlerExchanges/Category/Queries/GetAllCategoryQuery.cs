using CA.Core.Application.Contracts.Response;
using MediatR;

namespace CA.Core.Application.Contracts.HandlerExchanges.Category.Queries
{
    public class GetAllCategoryQuery : IRequest<PaginatedList<GetAllCategoryQueryResponse>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
