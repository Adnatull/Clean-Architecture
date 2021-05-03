using System.Collections.Generic;
using Core.Application.Contracts.Response;
using MediatR;

namespace Core.Application.Contracts.HandlerExchanges.Post.Queries
{
    public class GetAllPostQuery : IRequest<Response<IReadOnlyList<GetAllPostQueryResponse>>>
    {
    }
}
