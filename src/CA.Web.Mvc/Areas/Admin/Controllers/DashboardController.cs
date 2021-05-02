using CA.Core.Application.Contracts.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CA.Web.Mvc.Areas.Admin.Controllers
{
    /// <summary>
    /// Dashboard Controller
    /// </summary>

    [Area("Admin")]
    [Authorize]
    public class DashboardController : BaseController
    {
        /// <summary>
        /// Index Method
        /// </summary>
        /// <returns></returns>
        
        [Authorize(Policy = Permissions.Dashboards.View)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
