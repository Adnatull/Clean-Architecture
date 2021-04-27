using CA.Core.Application.Contracts.DataTransferObjects;
using CA.Core.Application.Contracts.Response;
using System.Threading.Tasks;

namespace CA.Core.Application.Contracts.Interfaces
{
    public interface IUserService
    {
        Task<PaginatedList<UserDto>> GetPaginatedUsersAsync(int? pageNumber, int? pageSize);
    }
}
