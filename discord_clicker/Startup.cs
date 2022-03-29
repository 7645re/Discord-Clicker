using discord_clicker.Models;
using discord_clicker.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using discord_clicker.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace discord_clicker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connection));
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => 
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });
            services.AddTransient<ItemHandler<Build, BuildModel>, ItemHandler<Build, BuildModel>>();
            services.AddTransient(
                serviceProvider =>
                {
                    IMemoryCache memoryCache = serviceProvider.GetService<IMemoryCache>();
                    ILogger<ItemHandlerCachingDecorator<Build, BuildModel>> logger = 
                        serviceProvider.GetService<ILogger<ItemHandlerCachingDecorator<Build, BuildModel>>>();
                    DatabaseContext db = serviceProvider.GetService<DatabaseContext>();
                    IItemHandler<Build, BuildModel> buildHandler =
                        serviceProvider.GetService<IItemHandler<Build, BuildModel>>();

                    IItemHandler<Build, BuildModel> cachingDecorator =
                        new ItemHandlerCachingDecorator<Build, BuildModel>(itemHandler: buildHandler, db: db, 
                            cache: memoryCache, logger: logger);
                    return cachingDecorator;
                }
            );
            services.AddTransient<UserHandler, UserHandler>();
            services.AddTransient<ItemHandler<Upgrade, UpgradeModel>, ItemHandler<Upgrade, UpgradeModel>>();
            services.AddTransient<ItemHandler<Achievement, AchievementModel>, ItemHandler<Achievement, AchievementModel>>();
            services.AddControllersWithViews();
            services.AddSignalR();
            services.AddMemoryCache();
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            // requires using Microsoft.AspNetCore.HttpOverrides;
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication(); 
            app.UseAuthorization();    
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
                endpoints.MapHub<ClickerHub>("/clicker");
            });
        }
    }
}