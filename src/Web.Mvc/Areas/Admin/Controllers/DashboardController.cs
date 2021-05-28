using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Framework.Permissions;

namespace Web.Mvc.Areas.Admin.Controllers
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
