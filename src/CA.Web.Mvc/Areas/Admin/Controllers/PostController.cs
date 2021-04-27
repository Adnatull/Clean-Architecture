using System.Threading.Tasks;
using CA.Core.Application.Contracts.HandlerExchanges.Post.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CA.Web.Mvc.Areas.Admin.Controllers
{
    /// <summary>
    /// Post Controller
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class PostController : BaseController
    {
        /// <summary>
        /// Index Method. Retrieve all Posts
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Add Post Get Endpoint
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add()
        {
            return View(new AddPostCommand());
        }

        /// <summary>
        /// Add Post HttpPost Endpoint
        /// </summary>
        /// <param name="addPostCommand"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> Add(AddPostCommand addPostCommand)
        {
            if (!ModelState.IsValid) return View(addPostCommand);
            var rs = await Mediator.Send(addPostCommand);
            if (rs.Succeeded)
                return RedirectToAction("Index", "Dashboard", new { id = rs.Data, message = rs.Message });
            return View(addPostCommand);
        }

        
    }
}
