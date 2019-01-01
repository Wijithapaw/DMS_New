using DMS.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.WebApi.Authorization
{
    public class ClaimAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        const string POLICY_PREFIX = "REQUIRE_CLAIM_";

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
            Task.FromResult(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var claim = policyName.Substring(POLICY_PREFIX.Length);

            var policy = new AuthorizationPolicyBuilder();

            policy.RequireClaim(DMSClaimTypes.Permission, claim);

            return Task.FromResult(policy.Build());
        }
    }
}
