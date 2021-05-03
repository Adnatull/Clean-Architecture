using Core.Application.Contracts.Response;
using MediatR;

namespace Core.Application.Contracts.HandlerExchanges.Post.Commands
{
    public class AddPostCommand : IRequest<Response<int>>
    {
        public string Title { get; set; }

        public string Slug { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
    }
}
