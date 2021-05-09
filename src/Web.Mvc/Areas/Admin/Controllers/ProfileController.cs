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
        private readonly IUserService _userService;
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// Profile Constructor
        /// </summary>
        public ProfileController(IUserService userService, ICurrentUser currentUser)
        {
            _userService = userService;
            _currentUser = currentUser;
        }

        /// <summary>
        /// Get Profile details of current user to View
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                userId = _currentUser.UserId;
            
            var rs = await _userService.GetUserDetailByIdAsync(userId);
            if(!rs.Succeeded)
                return RedirectToAction("Index", "Dashboard",
                    new { area = "Admin", id = rs.Data, succeeded = rs.Succeeded, message = rs.Message });
            return View(rs.Data);
        }

        /// <summary>
        /// Profile Update Method. Accept HttpPost data
        /// </summary>
        /// <param name="userDetailDto"></param>
        /// <returns>View</returns>
        [HttpPost]
        public async Task<IActionResult> Index(UserDetailDto userDetailDto)
        {
            if (!ModelState.IsValid) return View(userDetailDto);
            var rs = await _userService.UpdateUserProfile(userDetailDto);
            if (rs.Succeeded)
                return RedirectToAction("Index", "Profile",
                    new {area = "Admin", userId = userDetailDto.Id, id = rs.Data, succeeded = rs.Succeeded, message = rs.Message});
            ModelState.AddModelError(string.Empty, rs.Message);
            return View(userDetailDto);
        }
    }
}
