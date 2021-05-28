using AutoMapper;
using Core.Application.Contracts.AutomapperProfiles;
using Core.Application.Contracts.Interfaces;
using Core.Application.Services;
using Core.Domain.Identity.Constants;
using Core.Domain.Identity.CustomClaims;
using Core.Domain.Identity.Entities;
using Core.Domain.Identity.Interfaces;
using Core.Domain.Identity.Permissions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Framework.Permissions;

namespace UnitTest.Application.Services
{
    [TestFixture]
    public class UserServiceTest
    {
        private IUserService _userService;
        private IPermissionHelper _permissionHelper;


        private Mock<IApplicationUserManager> GetMockApplicationUserManager()
        {
            var applicationUserManager = new Mock<IApplicationUserManager>(MockBehavior.Strict);
            applicationUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(DefaultApplicationUsers.GetSuperUser);
            applicationUserManager.Setup(x => x.GetClaimsAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<Claim>() { new(CustomClaimTypes.Permission, Permissions.Posts.View) });
            applicationUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(DefaultApplicationRoles.GetDefaultRoles().Select(x => x.Name).ToList());
            return applicationUserManager;
        }

        private Mock<IApplicationRoleManager> GetMockApplicationRoleManager()
        {
            var applicationRoleManager = new Mock<IApplicationRoleManager>(MockBehavior.Strict);

            applicationRoleManager.Setup(x => x.GetClaimsAsync(It.IsAny<List<string>>()))
                .ReturnsAsync(_permissionHelper.GetAllPermissions());
            return applicationRoleManager;
        }
        private Mock<ICurrentUser> GetMockCurrentUser()
        {

            var currentUser = new Mock<ICurrentUser>(MockBehavior.Strict);
            currentUser.Setup(x => x.UserId).Returns(DefaultApplicationUsers.GetSuperUser().Id);
            currentUser.Setup(x => x.UserName).Returns(DefaultApplicationUsers.GetSuperUser().UserName);
            return currentUser;
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserServiceProfile());
            });
            _permissionHelper = new Web.Framework.Permissions.PermissionHelper();
            var mapper = mappingConfig.CreateMapper();
            var applicationUserManager = GetMockApplicationUserManager();

            var applicationRoleManager = GetMockApplicationRoleManager();
            var currentUser = GetMockCurrentUser();
   
           
            _userService = new UserService(applicationUserManager.Object, applicationRoleManager.Object, mapper, currentUser.Object, _permissionHelper);
        }

        


        [Test]
        public async Task GetAllClaimsTest()
        {
            var claimPrincipal =
                new ClaimsPrincipal(new ClaimsIdentity(_permissionHelper.GetAllPermissions(),
                    "AuthScheme"));
            var rs = await _userService.GetAllClaims(claimPrincipal);
            Assert.AreEqual(true, rs.Succeeded);
            Assert.GreaterOrEqual(rs.Data.Count, Decimal.ToInt32(1));
        }

        [Test]
        public async Task GetRolesAsyncAsync()
        {
            var claimPrincipal =
                new ClaimsPrincipal(new ClaimsIdentity(_permissionHelper.GetAllPermissions(),
                    "AuthScheme"));
            var rs = await _userService.GetRolesAsync(claimPrincipal);
            Assert.AreEqual(true, rs.Succeeded);
            Assert.AreEqual(4, rs.Data.Count);
        }
    }
}
