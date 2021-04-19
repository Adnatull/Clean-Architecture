using CA.Core.Application.Contracts.Response;
using MediatR;

namespace CA.Core.Application.Contracts.Handlers.Category.Queries
{
    public class GetAllCategoryQuery : IRequest<PaginatedList<GetAllCategoryQueryViewModel>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
