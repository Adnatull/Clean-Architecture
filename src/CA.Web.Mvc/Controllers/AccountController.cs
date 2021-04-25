using System.Threading.Tasks;
using CA.Core.Application.Contracts.DataTransferObjects;
using CA.Core.Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CA.Web.Mvc.Controllers
{
    /// <summary>
    /// Account controller. Responsible for functioning authentication related tasks 
    /// </summary>
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        /// <summary>
        /// Constructor. Injecting Dependencies
        /// </summary>
        /// <param name="accountService"></param>
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Register Action Method. User registration view renders from this action method
        /// </summary>
        /// <returns></returns>
        
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterUserDto());
        }


        /// <summary>
        /// Register Action Method. Registration POST request comes to this endpoint. 
        /// </summary>
        /// <param name="registerUserDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid) return View(registerUserDto);
            var rs = await _accountService.RegisterUserAsync(registerUserDto);
            if(rs.Succeeded)
                return RedirectToAction("Login", new {succeeded = rs.Succeeded, message = rs.Message});
            ModelState.AddModelError(string.Empty, rs.Message);
            return View(registerUserDto);
        }

        /// <summary>
        /// Login Action method. Login page will be rendered to view from this action method
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginUserDto());
        }

        /// <summary>
        /// Login Post Action method. This method will receive userName, password and will doe further process to complete login process
        /// </summary>
        /// <param name="loginUserDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
            if (!ModelState.IsValid) return View(loginUserDto);
            return View(loginUserDto);
        }
    }
}
