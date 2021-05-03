using Core.Application.Contracts.Response;
using MediatR;

namespace Core.Application.Contracts.HandlerExchanges.Post.Queries
{
    public class GetPostByIdQuery : IRequest<Response<GetPostByIdQueryResponse>>
    {
    }
}
