using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Application.Contracts.HandlerExchanges.Post.Commands;
using Core.Application.Contracts.Response;
using Core.Domain.Persistence.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Application.Handlers.Post
{
    public class PostCommandHandler : IRequestHandler<AddPostCommand, Response<int>>
    {
        private readonly ILogger<PostCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IPersistenceUnitOfWork _persistenceUnitOfWork;

        public PostCommandHandler(ILogger<PostCommandHandler> logger, IMapper mapper, IPersistenceUnitOfWork persistenceUnitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _persistenceUnitOfWork = persistenceUnitOfWork;
        }
        public async Task<Response<int>> Handle(AddPostCommand command, CancellationToken cancellationToken)
        {
            var post = _mapper.Map<Domain.Persistence.Entities.Post>(command);
            try
            {
                post = await _persistenceUnitOfWork.Post.AddAsync(post);
                await _persistenceUnitOfWork.CommitAsync();
            }
            catch(Exception e)
            {
                _persistenceUnitOfWork.Dispose();
                _logger.LogError(e, "Failed to save new post in database");
            }
            return Response<int>.Success(post.Id, "Successfully saved post");
        }

        
    }
}
