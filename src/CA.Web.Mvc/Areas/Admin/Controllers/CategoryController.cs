using CA.Core.Application.Contracts.Features.Category.Commands;
using CA.Core.Application.Contracts.Features.Category.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CA.Web.Mvc.Areas.Admin.Controllers
{
    /// <summary>
    /// Category Controller
    /// </summary>
    [Area("Admin")]
    public class CategoryController : BaseController
    {
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <param name="getAllCategory"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(GetAllCategoryQuery getAllCategory)
        {
            var rs = await Mediator.Send(getAllCategory);
            return View(rs);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new AddCategoryCommand());
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCategoryCommand add)
        {
            if (!ModelState.IsValid)
            {
                return View(add);
            }
            var rs = await Mediator.Send(add);
            if (rs.Succeeded)
                return RedirectToAction("Index", "Dashboard", new {id =rs.Data, message = rs.Message});
            ModelState.AddModelError(string.Empty, rs.Message);
            return View(add);
        }
    }
}
