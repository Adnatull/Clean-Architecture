using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Application.Contracts.HandlerExchanges.Category.Commands;
using Core.Application.Contracts.Response;
using Core.Domain.Persistence.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Core.Application.Handlers.Category
{
    public class CategoryCommandHandler : IRequestHandler<AddCategoryCommand, Response<int>>
    {
        private readonly IPersistenceUnitOfWork _persistenceUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryCommandHandler> _logger;

        public CategoryCommandHandler(IPersistenceUnitOfWork persistenceUnitOfWork,
                                        IMapper mapper,
                                        ILogger<CategoryCommandHandler> logger)
        {
            _persistenceUnitOfWork = persistenceUnitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<int>> Handle(AddCategoryCommand command, CancellationToken cancellationToken)
        {
            if (await _persistenceUnitOfWork.Category.Entity.Where(x => x.Slug == command.Slug)
                .AnyAsync(cancellationToken: cancellationToken))
                return Response<int>.Fail("The slug already exists. Please try a different one");
            var category = _mapper.Map<Domain.Persistence.Entities.Category>(command);
            try
            {
                await _persistenceUnitOfWork.Category.AddAsync(category);
                await _persistenceUnitOfWork.CommitAsync();
                return Response<int>.Success(category.Id, "Successfully added the category");
            }
            catch (Exception e)
            {
                _persistenceUnitOfWork.Dispose();
                _logger.LogError(e, "Failed to add category: {category} ", command.Title);
                return Response<int>.Fail("Failed to add the category");
            }
        }
    }
}
