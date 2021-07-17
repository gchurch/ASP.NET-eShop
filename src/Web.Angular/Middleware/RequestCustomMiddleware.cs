using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Angular.Middleware
{
    // Middleware is software that's assembled into an app pipeline to handle requests and responses.
    public class RequestCustomMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestCustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Each middleware component in the request pipeline is responsible for invoking the next component in the 
        // pipeline or short-circuiting the pipeline. Calling the "next" function passes the request to the next 
        // component. Middleware that doesn't call the next function is called terminal middleware and it 
        // short-circuits the pipeline.
        public async Task InvokeAsync(HttpContext httpContext)
        {
            Console.WriteLine("Hello from custom middleware.");
            await _next(httpContext);
        }
    }
}
