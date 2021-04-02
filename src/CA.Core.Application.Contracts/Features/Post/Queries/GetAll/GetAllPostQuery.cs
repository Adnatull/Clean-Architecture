using CA.Core.Application.Contracts.Response;
using MediatR;
using System.Collections.Generic;

namespace CA.Core.Application.Contracts.Features.Post.Queries.GetAll
{
    public class GetAllPostQuery : IRequest<Response<IReadOnlyList<GetAllPostQueryViewModel>>>
    {
    }
}
