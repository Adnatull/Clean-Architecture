using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CA.Core.Application.Contracts.DataTransferObjects;
using CA.Core.Application.Contracts.Interfaces;
using CA.Core.Application.Contracts.Response;
using CA.Core.Domain.Identity.Contracts;
using CA.Core.Domain.Identity.Entities;
using Microsoft.EntityFrameworkCore;

namespace CA.Core.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IApplicationRoleManager _roleManager;

        public RoleService(IApplicationRoleManager roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<PaginatedList<RoleDto>> GetPaginatedRolesAsync(int? pageNumber, int? pageSize)
        {
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<ApplicationRole, RoleDto>());
            var roles = _roleManager.Roles().ProjectTo<RoleDto>(configuration);
            return await PaginatedList<RoleDto>.CreateAsync(roles.AsNoTracking(),
                pageNumber ?? 1, pageSize ?? 12);
        }
    }
}
