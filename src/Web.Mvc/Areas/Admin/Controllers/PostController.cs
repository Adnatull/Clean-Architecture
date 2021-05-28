using Core.Application.Contracts.HandlerExchanges.Post.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Framework.Permissions;

namespace Web.Mvc.Areas.Admin.Controllers
{
    /// <summary>
    /// Post Controller
    /// </summary>
    [Area("Admin")]
    
    public class PostController : BaseController
    {
        /// <summary>
        /// Index Method. Retrieve all Posts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = Permissions.Posts.View)]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Add Post Get Endpoint
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = Permissions.Posts.Create)]
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
        [Authorize(Policy = Permissions.Posts.Create)]
        public async Task<IActionResult> Add(AddPostCommand addPostCommand)
        {
            if (!ModelState.IsValid) return View(addPostCommand);
            var rs = await Mediator.Send(addPostCommand);
            if (rs.Succeeded)
                return RedirectToAction("Index", 
                    new {area = "Admin", id = rs.Data, succeeded = rs.Succeeded, message = rs.Message});
            return View(addPostCommand);
        }

        
    }
}
