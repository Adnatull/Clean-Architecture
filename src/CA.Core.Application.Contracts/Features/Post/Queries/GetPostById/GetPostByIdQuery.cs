using CA.Core.Application.Contracts.Response;
using MediatR;

namespace CA.Core.Application.Contracts.Features.Post.Queries.GetPostById
{
    public class GetPostByIdQuery : IRequest<Response<GetPostByIdQueryViewModel>>
    {
    }
}
