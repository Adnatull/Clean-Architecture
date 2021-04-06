using CA.Core.Application.Contracts.Features.Category.Commands;
using CA.Core.Application.Contracts.Response;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CA.Core.Domain.Persistence.Contracts;
using Microsoft.Extensions.Logging;

namespace CA.Core.Application.Features.Category
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
            try
            {
                var category = _mapper.Map<Domain.Persistence.Entities.Category>(command);
                await _persistenceUnitOfWork.Category.AddAsync(category);
                await _persistenceUnitOfWork.CommitAsync();
                return Response<int>.Success(category.Id, "Successfully added the category");
            }
            catch (Exception e)
            {
                _persistenceUnitOfWork.Dispose();
                _logger.LogError(e, "Failed to add category: {category} ", command.Title);
            }
            return Response<int>.Fail("Failed to add the category");
        }
    }
}
