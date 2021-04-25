using AutoMapper;
using CA.Core.Application.Contracts.DataTransferObjects;
using CA.Core.Application.Contracts.Interfaces;
using CA.Core.Application.Contracts.Response;
using CA.Core.Domain.Identity.Contracts;
using CA.Core.Domain.Identity.Entities;
using System.Threading.Tasks;

namespace CA.Core.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IApplicationSignInManager _signInManager;
        private readonly IMapper _mapper;

        public AccountService(IApplicationUserManager userManager, IApplicationSignInManager signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
    }
}
