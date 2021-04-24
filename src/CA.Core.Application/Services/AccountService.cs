using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AutoMapper;
using CA.Core.Application.Contracts.DataTransferObjects;
using CA.Core.Application.Contracts.Interfaces;
using CA.Core.Application.Contracts.Response;
using CA.Core.Domain.Identity.Contracts;
using CA.Core.Domain.Identity.Entities;

namespace CA.Core.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IMapper _mapper;

        public AccountService(IApplicationUserManager userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<Response<UserIdentityDto>> RegisterUser(RegisterUserDto registerUserDto)
        {
            //var user = _mapper.Map<ApplicationUser>(registerUserDto);
            //var rs = await _userManager.RegisterUserAsync(user);
            //if (rs.Succeeded)
            //{
                
            //}
        }
    }
}
