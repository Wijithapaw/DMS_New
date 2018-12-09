using DMS.Utills.CustomClaims;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DMS.WebApi.AuthRequirements
{
    public class ManageSystemSettingsRequirment : IAuthorizationRequirement
    {
    }

    class ManageSystemSettingsHandler : AuthorizationHandler<ManageSystemSettingsRequirment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageSystemSettingsRequirment requirement)
        {
            if (AuthHelper.HasClaim(context, "system.settings.manage"))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
