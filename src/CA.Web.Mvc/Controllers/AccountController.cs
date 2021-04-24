using Microsoft.AspNetCore.Mvc;

namespace CA.Web.Mvc.Controllers
{
    /// <summary>
    /// Account controller. Responsible for functioning authentication related tasks 
    /// </summary>
 
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
