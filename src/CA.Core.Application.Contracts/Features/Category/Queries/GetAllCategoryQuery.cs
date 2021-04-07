using CA.Core.Application.Contracts.Response;
using MediatR;

namespace CA.Core.Application.Contracts.Features.Category.Queries
{
    public class GetAllCategoryQuery : IRequest<PaginatedList<GetAllCategoryQueryViewModel>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        
    }
}
