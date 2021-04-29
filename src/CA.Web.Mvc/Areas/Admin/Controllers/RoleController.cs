using System.Threading.Tasks;
using CA.Core.Application.Contracts.Interfaces;
using CA.Web.Framework.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CA.Web.Mvc.Areas.Admin.Controllers
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
    }
}
