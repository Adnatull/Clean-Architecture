using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Application.Contracts.HandlerExchanges.Post.Queries;
using Core.Application.Contracts.Response;
using MediatR;

namespace Core.Application.Handlers.Post
{
    public class PostQueryHandler : IRequestHandler<GetPostByIdQuery, Response<GetPostByIdQueryResponse>>,
                                    IRequestHandler<GetAllPostQuery, Response<IReadOnlyList<GetAllPostQueryResponse>>>
    {
        public async Task<Response<GetPostByIdQueryResponse>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Response<IReadOnlyList<GetAllPostQueryResponse>>> Handle(GetAllPostQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
