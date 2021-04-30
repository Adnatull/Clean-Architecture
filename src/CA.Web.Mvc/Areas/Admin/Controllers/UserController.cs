using System.Threading.Tasks;
using CA.Core.Application.Contracts.Interfaces;
using CA.Web.Framework.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CA.Web.Mvc.Areas.Admin.Controllers
{
    /// <summary>
    /// User Controller. User management functionalities can be done here
    /// </summary>
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Constructor. Dependency gets injected through this
        /// </summary>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Get All Users 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = Permissions.Users.View)]
        public async Task<IActionResult> Index(int? pageNumber, int? pageSize)
        {
            var rs = await _userService.GetPaginatedUsersAsync(pageNumber, pageSize);
            return View(rs);
        }
    }
}
