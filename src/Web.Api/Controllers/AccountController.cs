using Core.Application.Contracts.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Core.Application.Contracts.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using LinqToDB.Common;
using static Web.Framework.Permissions.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace Web.Api.Controllers {


    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase {

        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;

        public AccountController(IAccountService accountService, IConfiguration configuration) {
            _accountService = accountService;
            _configuration = configuration;
        }

        [HttpPost, Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginUserDtoForApi loginUserDto) {
            try {
                if (string.IsNullOrEmpty(loginUserDto.UserName) ||
                string.IsNullOrEmpty(loginUserDto.Password))
                    return BadRequest("Username and/or Password not specified");

                var rs = await _accountService.CheckPasswordAsync(loginUserDto);

                if (rs.Succeeded) {
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key")));

                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var claims = new[] {
                        new Claim(ClaimTypes.Name, loginUserDto.UserName),
                        new Claim(ClaimTypes.NameIdentifier, rs.Data.Id),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    var jwtSecurityToken = new JwtSecurityToken(

                        issuer: _configuration.GetValue<string>("Jwt:Issuer"),
                        audience: _configuration.GetValue<string>("Jwt:Audience"),
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(100),
                        signingCredentials: signinCredentials
                    );

                    return Ok(new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken));
                }
            }
            catch {
                return BadRequest
                ("An error occurred in generating the token");
            }
            return Unauthorized();
        }
    }
}
