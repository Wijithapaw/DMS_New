using DMS.Utills.CustomClaims;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.WebApi.AuthRequirements
{
    public static class AuthHelper
    {
        public static bool HasClaim(AuthorizationHandlerContext context, string claimType, string value)
        {
            return context.User.Claims.Any(c => c.Type == claimType && c.Value == value);
        }

        public static bool HasClaim(AuthorizationHandlerContext context, string value)
        {
            return HasClaim(context, CustomClaimTypes.Permission, value);
        }
    }
}
