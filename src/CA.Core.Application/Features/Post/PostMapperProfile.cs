using AutoMapper;
using CA.Core.Application.Contracts.Features.Post.Commands.Add;
using CA.Core.Application.Contracts.Features.Post.Queries.GetAll;
using CA.Core.Application.Contracts.Features.Post.Queries.GetPostById;

namespace CA.Core.Application.Features.Post
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
