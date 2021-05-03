using AutoMapper;
using Core.Application.Contracts.DataTransferObjects;
using Core.Domain.Identity.Entities;

namespace Core.Application.Contracts.AutomapperProfiles
{
    public class RoleServiceProfile : Profile
    {
        public RoleServiceProfile()
        {
            CreateMap<AddRoleDto, ApplicationRole>();
        }
    }
}
