using discord_clicker.Models;
using discord_clicker.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using discord_clicker.Services;
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
            services.AddTransient<UserHandler, UserHandler>();
            services.AddTransient<ItemHandler<Build, BuildModel, UserBuild>, ItemHandler<Build, BuildModel, UserBuild>>();
            services.AddTransient<ItemHandler<Upgrade, UpgradeModel, UserUpgrade>, ItemHandler<Upgrade, UpgradeModel, UserUpgrade>>();
            services.AddTransient<ItemHandler<Achievement, AchievementModel, UserAchievement>, ItemHandler<Achievement, AchievementModel, UserAchievement>>();
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