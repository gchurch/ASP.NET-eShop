using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Angular.Middleware
{
    public static class RequestCustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomMiddleware(
            this IApplicationBuilder builder
        )
        {
            return builder.UseMiddleware<RequestCustomMiddleware>();
        }
    }
}