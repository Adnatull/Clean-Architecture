using Core.Application.Contracts.DataTransferObjects;
using Core.Application.Contracts.Interfaces;
using Core.Application.Contracts.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Framework.Permissions;

namespace Web.Mvc.Areas.Admin.Controllers
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
        /// <param name="permissionValue"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = Permissions.Users.ManageClaims)]
        public async Task<IActionResult> ManageUserPermissions(string userId, string permissionValue, int? pageNumber, int? pageSize)
        {
            var rs = await _userService.ManagePermissionsAsync(userId, permissionValue, pageNumber, pageSize);
            if (!rs.Succeeded)
                return RedirectToAction("Index", "User", new { area = "Admin", succeeded = rs.Succeeded, message = rs.Message });
            return View(rs.Data);
        }


        /// <summary>
        /// Manage User Claims
        /// </summary>
        /// <param name="manageUserClaimDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = Permissions.Users.ManageClaims)]
        public async Task<IActionResult> ManageUserClaims(ManageUserClaimDto manageUserClaimDto)
        {
            if (!ModelState.IsValid)
                return Json(Response<UserIdentityDto>.Fail("Failed"));
            var rs = await _userService.ManageUserClaimAsync(manageUserClaimDto);
            return Json(rs);
        }
    }
}
