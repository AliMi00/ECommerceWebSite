using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Middlewares
{
    /// <summary>
    /// changes requests host name to real domain name.
    /// <strong>for use in reverse proxy and
    /// load balancer scenarios</strong>
    /// </summary>
    public class ChangeRequestUrl : IMiddleware
    {
        public ChangeRequestUrl()
        {
               
        }
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Request.Host = new HostString("https://www.google.com");

            return next.Invoke(context);

        }
    }
}
