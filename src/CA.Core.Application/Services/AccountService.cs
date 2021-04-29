using AutoMapper;
using CA.Core.Application.Contracts.DataTransferObjects;
using CA.Core.Application.Contracts.Interfaces;
using CA.Core.Application.Contracts.Response;
using CA.Core.Domain.Identity.Contracts;
using CA.Core.Domain.Identity.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CA.Core.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IApplicationSignInManager _signInManager;
        private readonly IApplicationRoleManager _roleManager;
        private readonly IMapper _mapper;

        public AccountService(IApplicationUserManager userManager,
                                IApplicationSignInManager signInManager,
                                IApplicationRoleManager roleManager,
                                IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public async Task<Response<UserIdentityDto>> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            var user = _mapper.Map<ApplicationUser>(registerUserDto);
            var rs = await _userManager.RegisterUserAsync(user);
            return rs.Succeeded
                ? Response<UserIdentityDto>.Success(new UserIdentityDto {Id = user.Id}, rs.ToString())
                : Response<UserIdentityDto>.Fail(rs.ToString());
        }

        public async Task<Response<UserIdentityDto>> CookieSignInAsync(LoginUserDto loginUserDto)
        {
            var user = await _userManager.GetUserByNameAsync(loginUserDto.UserName);
            if(user == null) return Response<UserIdentityDto>.Fail("UserName does not exists");
            var rs = await _signInManager.PasswordSignInAsync(user, loginUserDto.Password,
                loginUserDto.RememberMe, false);
            return rs.Succeeded
                ? Response<UserIdentityDto>.Success(new UserIdentityDto {Id = user.Id}, rs.ToString())
                : Response<UserIdentityDto>.Fail(rs.ToString());
        }

        public async Task<Response<IList<Claim>>> GetAllClaims(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            var userClaims = await _userManager.GetClaimsAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims =  await _roleManager.GetClaimsAsync(roles);

            var claims = userClaims.Union(roleClaims).ToList();
            return claims.Count > 0
                ? Response<IList<Claim>>.Success(claims, "Successfully retrieved")
                : Response<IList<Claim>>.Fail("No Claims found");
        }

        public async Task<Response<IList<string>>> GetRolesAsync(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            var roles = await _userManager.GetRolesAsync(user);
            return roles.Count > 0
                ? Response<IList<string>>.Success(roles, "Successfully retrieved")
                : Response<IList<string>>.Fail("No Roles found");
        }

        
    }
}
