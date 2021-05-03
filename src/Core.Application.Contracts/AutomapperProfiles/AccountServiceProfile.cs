using AutoMapper;
using Core.Application.Contracts.DataTransferObjects;
using Core.Domain.Identity.Entities;

namespace Core.Application.Contracts.AutomapperProfiles
{
    public class AccountServiceProfile : Profile
    {
        public AccountServiceProfile()
        {
            CreateMap<RegisterUserDto, ApplicationUser>();
        }
    }
}
