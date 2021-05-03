using AutoMapper;
using Core.Application.Contracts.DataTransferObjects;
using Core.Domain.Identity.Entities;

namespace Core.Application.Contracts.AutomapperProfiles
{
    public class UserServiceProfile : Profile
    {
        public UserServiceProfile()
        {
            CreateMap<ApplicationUser, UserDto>();
            CreateMap<ApplicationRole, ManageRoleDto>();
        }
    }
}
