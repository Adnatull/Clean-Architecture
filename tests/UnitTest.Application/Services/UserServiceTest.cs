using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Core.Application.Contracts.AutomapperProfiles;
using Core.Application.Contracts.Interfaces;
using Core.Application.Contracts.Permissions;
using Core.Application.Services;
using Core.Domain.Identity.Constants;
using Core.Domain.Identity.Contracts;
using Core.Domain.Identity.Entities;
using Moq;
using NUnit.Framework;

namespace UnitTest.Application.Services
{
    [TestFixture]
    public class UserServiceTest
    {
        private IUserService _userService;

        [OneTimeSetUp]
        public void SetUp()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserServiceProfile());
            });
            var mapper = mappingConfig.CreateMapper();
            var applicationUserManager = new Mock<IApplicationUserManager>(MockBehavior.Strict);
            applicationUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(DefaultApplicationUsers.GetSuperUser);
            applicationUserManager.Setup(x => x.GetClaimsAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<Claim>() { new(CustomClaimTypes.Permission, Permissions.Posts.View) });
            applicationUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(DefaultApplicationRoles.GetDefaultRoles().Select(x => x.Name).ToList());
            var applicationRoleManager = new Mock<IApplicationRoleManager>(MockBehavior.Strict);

            applicationRoleManager.Setup(x => x.GetClaimsAsync(It.IsAny<List<string>>()))
                .ReturnsAsync(PermissionHelper.GetPermissionClaims());
            _userService = new UserService(applicationUserManager.Object, applicationRoleManager.Object, mapper);
        }
        [Test]
        public async Task GetAllClaimsTest()
        {
            var claimPrincipal =
                new ClaimsPrincipal(new ClaimsIdentity(PermissionHelper.GetPermissionClaims(),
                    "AuthScheme"));
            var rs = await _userService.GetAllClaims(claimPrincipal);
            Assert.AreEqual(true, rs.Succeeded);
            Assert.GreaterOrEqual(rs.Data.Count, Decimal.ToInt32(1));
        }

        [Test]
        public async Task GetRolesAsyncAsync()
        {
            var claimPrincipal =
                new ClaimsPrincipal(new ClaimsIdentity(PermissionHelper.GetPermissionClaims(),
                    "AuthScheme"));
            var rs = await _userService.GetRolesAsync(claimPrincipal);
            Assert.AreEqual(true, rs.Succeeded);
            Assert.AreEqual(4, rs.Data.Count);
        }
    }
}
