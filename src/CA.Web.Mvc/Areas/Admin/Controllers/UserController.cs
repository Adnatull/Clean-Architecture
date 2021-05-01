using CA.Core.Application.Contracts.DataTransferObjects;
using CA.Core.Application.Contracts.Interfaces;
using CA.Core.Application.Contracts.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        /// <summary>
        /// Manage User Roles
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = Permissions.Users.ManageRoles)]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            var rs = await _userService.ManageRolesAsync(userId);
            if (!rs.Succeeded)
                return RedirectToAction("Index", "User", new { area = "Admin", succeeded = rs.Succeeded, message = rs.Message });
            return View(rs.Data);
        }

        /// <summary>
        /// Manage User Roles. Post Method
        /// </summary>
        /// <param name="manageUserRolesDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = Permissions.Users.ManageRoles)]
        public async Task<IActionResult> ManageUserRoles(ManageUserRolesDto manageUserRolesDto)
        {
            if (!ModelState.IsValid) return View(manageUserRolesDto);
            var rs = await _userService.ManageRolesAsync(manageUserRolesDto);
            if (rs.Succeeded)
                return RedirectToAction("Index", "User", new { area = "Admin", id = rs.Data.Id, succeeded = rs.Succeeded, message = rs.Message });
            ModelState.AddModelError(string.Empty, rs.Message);
            return View(manageUserRolesDto);
        }

        /// <summary>
        /// Manage User Permissions
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = Permissions.Users.ManagePermissions)]
        public async Task<IActionResult> ManageUserPermissions(string userId)
        {
            var rs = await _userService.ManagePermissionsAsync(userId);
            if (!rs.Succeeded)
                return RedirectToAction("Index", "User", new { area = "Admin", succeeded = rs.Succeeded, message = rs.Message });
            return View(rs.Data);
        }

        /// <summary>
        ///  Manage User Permissions. Post Method
        /// </summary>
        /// <param name="manageUserPermissionsDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = Permissions.Users.ManagePermissions)]
        public async Task<IActionResult> ManageUserPermissions(ManageUserPermissionsDto manageUserPermissionsDto)
        {
            if (!ModelState.IsValid) return View(manageUserPermissionsDto);
            var rs = await _userService.ManagePermissionsAsync(manageUserPermissionsDto);
            if (rs.Succeeded)
                return RedirectToAction("Index", "User", new { area = "Admin", id = rs.Data.Id, succeeded = rs.Succeeded, message = rs.Message });
            ModelState.AddModelError(string.Empty, rs.Message);
            return View(manageUserPermissionsDto);
        }
    }
}
