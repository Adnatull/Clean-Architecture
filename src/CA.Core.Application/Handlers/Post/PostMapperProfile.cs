using AutoMapper;
using CA.Core.Application.Contracts.Handlers.Post.Commands;
using CA.Core.Application.Contracts.Handlers.Post.Queries;

namespace CA.Core.Application.Handlers.Post
{
    public class PostMapperProfile : Profile
    {
        public PostMapperProfile()
        {
            CreateMap<Domain.Persistence.Entities.Post, GetPostByIdQueryViewModel>();
            CreateMap<Domain.Persistence.Entities.Post, GetAllPostQueryViewModel>();
            CreateMap<AddPostCommand, Domain.Persistence.Entities.Post>();
        }
    }
}
