using System.Collections.Generic;
using CA.Core.Application.Contracts.Response;
using MediatR;

namespace CA.Core.Application.Contracts.HandlerExchanges.Post.Queries
{
    public class GetAllPostQuery : IRequest<Response<IReadOnlyList<GetAllPostQueryViewModel>>>
    {
    }
}
