using DMS.Utills.CustomClaims;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DMS.WebApi.AuthRequirements
{
    public class ManageProjectRequirement : IAuthorizationRequirement
    {
    }

    public class ManageProjectHandler : AuthorizationHandler<ManageProjectRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageProjectRequirement requirement)
        {
            if (AuthHelper.HasClaim(context, "projects.edit"))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
