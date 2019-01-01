using DMS.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DMS.WebApi.Middlewares
{
    public class RequestContextResolveMiddleware
    {
        private readonly RequestDelegate next;

        public RequestContextResolveMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, IRequestContext requestContext /* other scoped dependencies */)
        {
            var userIdStr = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userId = 0;
            int.TryParse(userIdStr, out userId);

            requestContext.UserId = userId;

            await next(context);
        }
    }
}
