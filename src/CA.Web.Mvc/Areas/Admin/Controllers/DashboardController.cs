using Microsoft.AspNetCore.Mvc;

namespace CA.Web.Mvc.Areas.Admin.Controllers
{
    /// <summary>
    /// Dashboard Controller
    /// </summary>

    [Area("Admin")]
    
    public class DashboardController : BaseController
    {
        /// <summary>
        /// Index Method
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}
