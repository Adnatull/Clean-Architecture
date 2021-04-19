﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CA.Core.Application.Contracts.Handlers.Category.Commands;
using CA.Core.Application.Contracts.Handlers.Category.Queries;

namespace CA.Web.Mvc.Areas.Admin.Controllers
{
    /// <summary>
    /// Category Controller
    /// This controller does handle requests related to Catgegory. It add, delete, update, fetch categories. It also does filter based fetch.
    /// </summary>
    [Area("Admin")]
    public class CategoryController : BaseController
    {
        /// <summary>
        /// Get All Categories. This method is responsible for fetching all categories. Here GetAllCategoryQuery pass some parameter like PageSize, PageNumber
        /// for papginated results.
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
