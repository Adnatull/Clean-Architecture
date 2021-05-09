using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Application.Contracts.HandlerExchanges.Post.Commands;
using Core.Application.Handlers.Post;
using Core.Domain.Persistence.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace UnitTest.Application.Handlers.Post
{
    [TestFixture]
    public class PostCommandHandlerTest
    {
        
        private PostCommandHandler _postCommandHandler;
        [OneTimeSetUp]
        public void SetUp()
        {
            var logger = new Mock<ILogger<PostCommandHandler>>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new PostMapperProfile());
            });
            var mapper = mappingConfig.CreateMapper();
            var persistenceUnitOfWork = new Mock<IPersistenceUnitOfWork>(MockBehavior.Strict);
            persistenceUnitOfWork.Setup(x => x.Post.AddAsync(It.IsAny<Core.Domain.Persistence.Entities.Post>())).ReturnsAsync(new Core.Domain.Persistence.Entities.Post{Id = 1});
            persistenceUnitOfWork.Setup(x => x.CommitAsync());
            persistenceUnitOfWork.Setup(x => x.Dispose());
            _postCommandHandler = new PostCommandHandler(logger.Object, mapper, persistenceUnitOfWork.Object);
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
