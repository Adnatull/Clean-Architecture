using AutoMapper;
using CA.Core.Application.Contracts.DataTransferObjects;
using CA.Core.Domain.Identity.Entities;

namespace CA.Core.Application.Contracts.AutomapperProfiles
{
    public class UserServiceProfile : Profile
    {
        public UserServiceProfile()
        {
            CreateMap<ApplicationUser, UserDto>();
            CreateMap<ApplicationRole, ManageRolesDto>();
        }
    }
}
