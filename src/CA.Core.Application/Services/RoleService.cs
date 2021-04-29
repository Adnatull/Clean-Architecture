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
        private readonly IMapper _mapper;

        public RoleService(IApplicationRoleManager roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public async Task<PaginatedList<RoleDto>> GetPaginatedRolesAsync(int? pageNumber, int? pageSize)
        {
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<ApplicationRole, RoleDto>());
            var roles = _roleManager.Roles().ProjectTo<RoleDto>(configuration);
            return await PaginatedList<RoleDto>.CreateAsync(roles.AsNoTracking(),
                pageNumber ?? 1, pageSize ?? 12);
        }

        public async Task<Response<string>> AddRoleAsync(AddRoleDto addRoleDto)
        {
            if(await _roleManager.GetRoleAsync(addRoleDto.Name) != null)
                return Response<string>.Fail("The role already exists. Please try a different one!");
            var appRole = _mapper.Map<ApplicationRole>(addRoleDto);
            var rs = await _roleManager.AddRoleAsync(appRole);
            return rs.Succeeded
                ? Response<string>.Success(appRole.Id, "New role has been created")
                : Response<string>.Fail("Failed to create new role");
        }
    }
}
