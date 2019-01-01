using DMS.Domain.CustomExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DMS.WebApi.Middlewares
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorLoggingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<ErrorLoggingMiddleware> logger /* other scoped dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                LogError(ex, logger);
                throw;
            }
        }

        private static void LogError(Exception ex, ILogger<ErrorLoggingMiddleware> logger)
        {
            if (ex is DMSException)
            {
                logger.LogWarning(ex, ex.Message);
            }
            else
            {
                logger.LogError(ex, "Unhandled error");
            }
        } 
    }
}
