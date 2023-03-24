using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Application.Contracts.HandlerExchanges.Post.Queries;
using Core.Application.Contracts.Response;
using Core.Domain.Persistence.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Application.Handlers.Post
{
    public class PostQueryHandler : IRequestHandler<GetPostByIdQuery, Response<GetPostByIdQueryResponse>>,
                                    IRequestHandler<GetAllPostQuery, Response<IReadOnlyList<GetAllPostQueryResponse>>>
    {
        private readonly ILogger<PostQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IPersistenceUnitOfWork _persistenceUnitOfWork;

        public PostQueryHandler(ILogger<PostQueryHandler> logger, IMapper mapper, IPersistenceUnitOfWork persistenceUnitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _persistenceUnitOfWork = persistenceUnitOfWork;
        }
        public async Task<Response<GetPostByIdQueryResponse>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Response<IReadOnlyList<GetAllPostQueryResponse>>> Handle(GetAllPostQuery request, CancellationToken cancellationToken)
        {
            var posts = await _persistenceUnitOfWork.Post.GetAllAsync();
            var postsDto = _mapper.Map<List<GetAllPostQueryResponse>>(posts);
            return Response<IReadOnlyList<GetAllPostQueryResponse>>.Success(postsDto, "success");
        }
    }
}
