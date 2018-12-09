using DMS.Utills.CustomClaims;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.WebApi.AuthRequirements
{
    public class ManageAccountsRequirment : IAuthorizationRequirement
    {
    }

    class ManageAccountsHandler : AuthorizationHandler<ManageAccountsRequirment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAccountsRequirment requirement)
        {
            if (AuthHelper.HasClaim(context, "accounts.manage"))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
