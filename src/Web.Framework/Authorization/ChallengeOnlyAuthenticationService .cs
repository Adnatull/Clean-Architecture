//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Http;
//using System;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace Web.Framework.Authorization
//{

//    //services.AddSingleton<IAuthenticationService, ChallengeOnlyAuthenticationService>();
//    public class ChallengeOnlyAuthenticationService : IAuthenticationService
//    {
//        public async Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string? scheme)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task ChallengeAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task ForbidAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task SignInAsync(HttpContext context, string? scheme, ClaimsPrincipal principal, AuthenticationProperties? properties)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task SignOutAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
