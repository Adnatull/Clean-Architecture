﻿using CA.Core.Domain.Identity.Response;
using Microsoft.AspNetCore.Identity;

namespace CA.Core.Domain.Identity.Extensions
{
    public static class IdentityExtensions
    {
        public static IdentityResponse ToIdentityResponse(this IdentityResult identityResult)
        {
            return identityResult.Succeeded
                ? IdentityResponse.Success(identityResult.ToString())
                : IdentityResponse.Fail(identityResult.ToString());
        }
    }
}