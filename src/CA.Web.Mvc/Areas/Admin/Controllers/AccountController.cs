using Microsoft.AspNetCore.Mvc;

namespace CA.Web.Mvc.Areas.Admin.Controllers
{
    /// <summary>
    /// Account controller. Responsible for functioning authentication related tasks 
    /// </summary>
    [Area("Admin")]
    public class AccountController : BaseController
    {
        /// <summary>
        /// Register Action Method. User registration view renders from this action method
        /// </summary>
        /// <returns></returns>
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
    }
}
