using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ECommerceWebSite.Models.DbModels;
using ECommerceWebSite.Data;
using ECommerceWebSite.Middlewares;
using ECommerceWebSite.Services;

namespace ECommerceWebSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //use debug db 
            string ConnectionString = "";
#if DEBUG
            ConnectionString = Configuration.GetConnectionString("ConnectionStringDebug");
#else
            ConnectionString = Configuration.GetConnectionString("ConnectionStringRelease");
#endif

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(ConnectionString));

            services.AddDefaultIdentity<Customer>(options =>
            {

                //reduce security for development 
                //TODO : change before release 
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.User.RequireUniqueEmail = false;

                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
            });

            services.AddControllersWithViews();
            services.AddRazorPages();
            //services 
            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
            services.AddTransient<IProductServices, ProductServices>();
            services.AddTransient<ICategoryServices, CategoryServices>();
            services.AddTransient<IOrderServices, OrderServices>();
            services.AddTransient<ICartServices, CartServices>();
            services.AddTransient<IAdminServices, AdminServices>();
            services.AddTransient<ChangeRequestUrl>();
            services.AddHostedService<DatabaseGuard>();
            services.AddHostedService<TempCartCleanUp>();
            services.AddMvc().AddRazorRuntimeCompilation();


            


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {



            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();

            }
            else
            {
                //Change Url Middleware for reverce proxy 
                app.ChangeRequestUrl();

                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
