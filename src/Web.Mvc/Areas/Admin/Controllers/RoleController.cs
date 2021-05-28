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
    /// Adding, updating functionality of roles can be done from this controller
    /// </summary>
    [Area("Admin")]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        /// <summary>
        /// Constructor method. RoleService is injected here
        /// </summary>
        /// <param name="roleService"></param>
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Retrieve all roles and render them to View
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = Permissions.Roles.View)]
        public async Task<IActionResult> Index(int? pageNumber, int? pageSize)
        {
            var rs = await _roleService.GetPaginatedRolesAsync(pageNumber, pageSize);
            return View(rs);
        }

        /// <summary>
        /// Render view to create role
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = Permissions.Roles.Create)]
        public IActionResult Add()
        {
            return View(new AddRoleDto());
        }

        /// <summary>
        /// Endpoint to create new role. This accept role data from user and submit to respective service
        /// </summary>
        /// <param name="addRoleDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = Permissions.Roles.Create)]
        public async Task<IActionResult> Add(AddRoleDto addRoleDto)
        {
            if (!ModelState.IsValid) return View(addRoleDto);
            var rs = await _roleService.AddRoleAsync(addRoleDto);
            if (rs.Succeeded)
                return RedirectToAction("Index", "Role",
                    new {area = "Admin", id = rs.Data, succeeded = rs.Succeeded, message = rs.Message});
            ModelState.AddModelError(string.Empty, rs.Message);
            return View(addRoleDto);
        }

        /// <summary>
        /// Manage Role Claims/Permissions
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permissionValue"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = Permissions.Roles.ManageClaims)]
        public async Task<IActionResult> ManageRolePermissions(string roleId, string permissionValue, int? pageNumber, int? pageSize)
        {
            var rs = await _roleService.ManagePermissionsAsync(roleId, permissionValue, pageNumber, pageSize);
            if (!rs.Succeeded)
                return RedirectToAction("Index", "Role", new { area = "Admin", succeeded = rs.Succeeded, message = rs.Message });
            return View(rs.Data);
        }


        /// <summary>
        /// Manage Role Claims
        /// </summary>
        /// <param name="manageRoleClaimDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = Permissions.Roles.ManageClaims)]
        public async Task<IActionResult> ManageRoleClaims(ManageRoleClaimDto manageRoleClaimDto)
        {
            if (!ModelState.IsValid)
                return Json(Response<RoleIdentityDto>.Fail( "Failed"));
            var rs = await _roleService.ManageRoleClaimAsync(manageRoleClaimDto);
            return Json(rs);
        }
        
    }
}
