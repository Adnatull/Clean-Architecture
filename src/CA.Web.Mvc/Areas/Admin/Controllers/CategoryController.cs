using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CA.Core.Application.Contracts.Features.Category.Commands;

namespace CA.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : BaseController
    {
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
