using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Middlewares
{

    public static class ChangeRequestUrlExtention
    {
        public static IApplicationBuilder ChangeRequestUrl(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ChangeRequestUrl>();
        }
    }
}
