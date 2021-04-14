using System;
using System.Threading;
using AutoMapper;
using CA.Core.Application.Features.Post;
using CA.Core.Domain.Persistence.Contracts;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System.Threading.Tasks;
using CA.Core.Application.Contracts.Features.Post.Commands.Add;
using Moq;

namespace CA.Tests.ApplicationServices.Features.Post
{
    [TestFixture]
    public class PostCommandHandlerTest
    {
        private Mock<ILogger<PostCommandHandler>> _logger;
        private IMapper _mapper;
        private Mock<IPersistenceUnitOfWork> _persistenceUnitOfWork;
        private PostCommandHandler _postCommandHandler;
        [OneTimeSetUp]
        public void SetUp()
        {
            _logger = new Mock<ILogger<PostCommandHandler>>();
            _persistenceUnitOfWork = new Mock<IPersistenceUnitOfWork>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new PostMapperProfile());
            });
            var mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
            _persistenceUnitOfWork = new Mock<IPersistenceUnitOfWork>(MockBehavior.Strict);
            _persistenceUnitOfWork.Setup(x => x.Post.AddAsync(It.IsAny<Core.Domain.Persistence.Entities.Post>())).ReturnsAsync(new Core.Domain.Persistence.Entities.Post{Id = 1});
            _persistenceUnitOfWork.Setup(x => x.CommitAsync());
            _persistenceUnitOfWork.Setup(x => x.Dispose());
            _postCommandHandler = new PostCommandHandler(_logger.Object, _mapper, _persistenceUnitOfWork.Object);
        }

        [Test]
        public async Task AddPostCommandTest()
        {
            var post = new AddPostCommand
            {
                Title = "first-post",
                Slug = "first-post",
                Summary = "This is a summery",
                Content = "This is a content"
            };

            var rs = await _postCommandHandler.Handle(post, CancellationToken.None);
            Assert.AreEqual(rs.Message, "Successfully saved post");
            Assert.AreEqual(rs.Data, 1);
        }
    }
}
