using Core.Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Core.Application.Contracts.DataTransferObjects;

namespace Web.Mvc.Areas.Admin.Controllers
{
    /// <summary>
    /// Profile Controller
    /// </summary>
    [Area("Admin")]
    [Authorize]
    public class ProfileController : BaseController
    {
        /// <summary>
        /// Profile Constructor
        /// </summary>
        public ProfileController()
        {
            
        }
        /// <summary>
        /// Get Profile details of current user to View
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
