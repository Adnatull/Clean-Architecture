using System.Threading.Tasks;
using CA.Core.Application.Contracts.DataTransferObjects;
using CA.Core.Application.Contracts.Response;

namespace CA.Core.Application.Contracts.Interfaces
{
    public interface IAccountService
    {
        Task<Response<UserIdentityDto>> RegisterUser(RegisterUserDto registerUserDto);
    }
}
