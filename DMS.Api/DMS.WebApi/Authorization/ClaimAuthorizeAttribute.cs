using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.WebApi.Authorization
{
    public class ClaimAuthorizeAttribute : AuthorizeAttribute
    {
        const string POLICY_PREFIX = "REQUIRE_CLAIM_";

        public ClaimAuthorizeAttribute(string claim) => Claim = claim;

        // Get or set the Age property by manipulating the underlying Policy property
        public string Claim
        {
            get
            {
                var claim = Policy.Substring(POLICY_PREFIX.Length);
                return claim;
            }
            set
            {
                Policy = $"{POLICY_PREFIX}{value}";
            }
        }
    }
}
