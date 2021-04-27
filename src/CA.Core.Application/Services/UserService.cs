using AutoMapper;
using CA.Core.Application.Contracts.DataTransferObjects;
using CA.Core.Application.Contracts.Interfaces;
using CA.Core.Application.Contracts.Response;
using CA.Core.Domain.Identity.Contracts;
using CA.Core.Domain.Identity.Entities;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using CA.Core.Application.Contracts.HandlerExchanges.Category.Queries;
using Microsoft.EntityFrameworkCore;

namespace CA.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationUserManager _userManager;

        public UserService(IApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<PaginatedList<UserDto>> GetPaginatedUsersAsync(int? pageNumber, int? pageSize)
        {
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<ApplicationUser, UserDto>());
            var cats = _userManager.Users().ProjectTo<UserDto>(configuration);
            return await PaginatedList<UserDto>.CreateAsync(cats.AsNoTracking(),
                pageNumber ?? 1, pageSize ?? 12);
        }
    }
}
