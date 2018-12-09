using DMS.Utills;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DMS.WebApi.Utills
{
    public class WebEnvironmentDescriptor : IEnvironmentDescriptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public int UserId { get; private set; }

        public WebEnvironmentDescriptor(IHttpContextAccessor httpContextAccessot)
        {
            _httpContextAccessor = httpContextAccessot;
            Initialize();
        }

        private void Initialize()
        {
            var userIdStr = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr != null)
                UserId = int.Parse(userIdStr);
        }
    }
}
