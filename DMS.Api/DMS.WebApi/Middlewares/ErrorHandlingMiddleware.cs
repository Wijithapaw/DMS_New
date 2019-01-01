using DMS.Domain.CustomExceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DMS.WebApi.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other scoped dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string errorMsg = "";
            var errorCode = HttpStatusCode.InternalServerError;

            if (exception is RecordNotFoundException)
            {
                errorMsg = exception.Message;
                errorCode = HttpStatusCode.NotFound;
            }
            else if (exception is DMSException)
            {
                errorMsg = exception.Message;
                errorCode = HttpStatusCode.BadRequest;
            }
            else
            {
                errorMsg = "Unknown Error";
                errorCode = HttpStatusCode.InternalServerError;
            }

            var result = JsonConvert.SerializeObject(new { errorMessage = errorMsg });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)errorCode;
            return context.Response.WriteAsync(result);
        }
    }
}
