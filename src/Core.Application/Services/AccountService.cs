using AutoMapper;
using Core.Application.Contracts.DataTransferObjects;
using Core.Application.Contracts.Interfaces;
using Core.Application.Contracts.Response;
using Core.Domain.Identity.Entities;
using System.Threading.Tasks;
using Core.Domain.Identity.Interfaces;

namespace Core.Application.Services
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

        public async Task CookieSignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<Response<UserIdentityDto>> CheckPasswordAsync(LoginUserDtoForApi loginUserDto) {
            var user = await _userManager.GetUserByNameAsync(loginUserDto.UserName);
            if (user == null) return Response<UserIdentityDto>.Fail("UserName does not exists");

            var rs = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);
            return rs.Succeeded
                ? Response<UserIdentityDto>.Success(new UserIdentityDto { Id = user.Id }, rs.ToString())
                : Response<UserIdentityDto>.Fail(rs.ToString());
        }
    }
}
