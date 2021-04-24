using AutoMapper;
using CA.Core.Application.Contracts.DataTransferObjects;
using CA.Core.Domain.Identity.Entities;

namespace CA.Core.Application.Contracts.AutomapperProfiles
{
    public class AccountServiceProfile : Profile
    {
        public AccountServiceProfile()
        {
            CreateMap<RegisterUserDto, ApplicationUser>();
        }
    }
}
