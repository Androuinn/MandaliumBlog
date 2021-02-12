using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Mandalium.API.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class RequestCultureMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestCultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {

            if (httpContext.Request != null)
            {
                string language = httpContext.Request.Headers["lang"];

                try
                {
                    CultureInfo.CurrentUICulture = new CultureInfo(language);
                }
                catch
                {
                    CultureInfo.CurrentUICulture = new CultureInfo("tr-TR");
                }
            }

            httpContext.Response.Headers.Remove("Server");
            httpContext.Response.Headers.Remove("X-Powered-By");
            httpContext.Response.Headers.Add("X-Frame-Options", "DENY");
            httpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            httpContext.Response.Headers.Add("X-Xss-Protection", "1; mode=block");

            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestCultureMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestCultureMiddleware>();
        }
    }
}
