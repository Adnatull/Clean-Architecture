using AutoMapper;
using Core.Application.Contracts.AutomapperProfiles;
using Core.Application.Contracts.DataTransferObjects;
using Core.Application.Contracts.Interfaces;
using Core.Application.Services;
using Core.Domain.Identity.Constants;
using Core.Domain.Identity.Entities;
using Core.Domain.Identity.Response;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Core.Domain.Identity.Interfaces;

namespace UnitTest.Application.Services
{
    [TestFixture]
    public class AccountServiceTest
    {
        private IAccountService _accountService;

        private Mock<IApplicationUserManager> GetMockApplicationUserManager()
        {
            var applicationUserManager = new Mock<IApplicationUserManager>(MockBehavior.Strict);
            applicationUserManager.Setup(x => x.RegisterUserAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResponse.Success("Succeeded"));
            applicationUserManager.Setup(x => x.GetUserByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(DefaultApplicationUsers.GetSuperUser());
            return applicationUserManager;
        }

        private Mock<IApplicationSignInManager> GetMockApplicationSignInManager()
        {
            var applicationSignInManager = new Mock<IApplicationSignInManager>(MockBehavior.Strict);
            applicationSignInManager.Setup(x =>
                    x.PasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(IdentityResponse.Success("Succeeded"));
            applicationSignInManager.Setup(x => x.SignOutAsync());
            return applicationSignInManager;
        }

        private Mock<IApplicationRoleManager> GetMockApplicationRoleManager()
        {
            var applicationRoleManager = new Mock<IApplicationRoleManager>(MockBehavior.Strict);
            return applicationRoleManager;
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AccountServiceProfile());
            });
            var mapper = mappingConfig.CreateMapper();

            var applicationUserManager = GetMockApplicationUserManager();

            var applicationSignInManager = GetMockApplicationSignInManager();

            var applicationRoleManager = GetMockApplicationRoleManager();

            _accountService = new AccountService(applicationUserManager.Object, applicationSignInManager.Object,
                applicationRoleManager.Object, mapper);
        }

        


        [Test]
        public async Task RegisterUserAsyncTest()
        {
            var registerUserDto = new RegisterUserDto
            {
                FirstName = DefaultApplicationUsers.GetSuperUser().FirstName,
                LastName = DefaultApplicationUsers.GetSuperUser().LastName,
                Email = DefaultApplicationUsers.GetSuperUser().Email,
                Password = "SuperAdmin",
                ConfirmPassword = "SuperAdmin"
            };
            var rs = await _accountService.RegisterUserAsync(registerUserDto);
            Assert.AreEqual(true, rs.Succeeded);
        }

        [Test]
        public async Task CookieSignInAsyncTest()
        {
            var loginUserDto = new LoginUserDto
            {
                UserName = DefaultApplicationUsers.GetSuperUser().UserName,
                Password = "SuperAdmin",
                RememberMe = true
            };
            var rs = await _accountService.CookieSignInAsync(loginUserDto);
            Assert.AreEqual(true, rs.Succeeded);
        }

        
    }
}
