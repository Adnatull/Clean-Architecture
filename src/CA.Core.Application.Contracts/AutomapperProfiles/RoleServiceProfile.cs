using AutoMapper;
using CA.Core.Application.Contracts.DataTransferObjects;
using CA.Core.Domain.Identity.Entities;

namespace CA.Core.Application.Contracts.AutomapperProfiles
{
    public class RoleServiceProfile : Profile
    {
        public RoleServiceProfile()
        {
            CreateMap<AddRoleDto, ApplicationRole>();
        }
    }
}
