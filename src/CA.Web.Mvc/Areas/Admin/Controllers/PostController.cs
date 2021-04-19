using System.Threading.Tasks;
using CA.Core.Application.Contracts.HandlerExchanges.Post.Commands;
using Microsoft.AspNetCore.Mvc;

namespace CA.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new AddPostCommand());
        }

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
