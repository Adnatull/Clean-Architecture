using CA.Core.Application.Contracts.Response;
using MediatR;

namespace CA.Core.Application.Contracts.HandlerExchanges.Post.Queries
{
    public class GetPostByIdQuery : IRequest<Response<GetPostByIdQueryViewModel>>
    {
    }
}
