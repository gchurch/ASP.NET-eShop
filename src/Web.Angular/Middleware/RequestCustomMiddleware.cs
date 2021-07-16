using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Angular.Middleware
{
    public class RequestCustomMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestCustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            Console.WriteLine("Hello from custom middleware.");
            await _next(httpContext);
        }
    }
}
