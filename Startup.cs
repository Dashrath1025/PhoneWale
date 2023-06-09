using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopify.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace Shopify
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
            
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                });

            services.AddControllersWithViews();
            services.AddDbContext<ShopifyDbContext>(context => context.UseSqlServer(Configuration.GetConnectionString("ShopifyDBConnection")));
            services.AddScoped<ICategory, SqlCategoryRepository>();
            //   services.AddSingleton<ICategory, CategoryRepository>();
            //services.AddSingleton<ISubCategory, SubCategoryRepository>();
            services.AddScoped<ISubCategory, SqlSubCategoryRepository>();
            services.AddScoped<IProduct, SQLProductRepository>();
            
            services.AddScoped<IUserLike, SqlUserLike>();
            services.AddScoped<IRecently, SqlRecently>();
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ShopifyDbContext>();
             services.Configure<IdentityOptions>(option => {
                 option.Password.RequiredLength = 6;
                 option.Password.RequireNonAlphanumeric = false;
                 option.Password.RequireUppercase = false;
             });
            services.AddSession(options =>
            {
                options.Cookie.Name = ".MyApp.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(10);
            });



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseSession();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Visitor}/{action=Index}/{id?}");
            });

        }
    }
}
