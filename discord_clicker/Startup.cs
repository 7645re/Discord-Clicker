using discord_clicker.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using discord_clicker.Services.UserHandler;
using discord_clicker.Models.Items.AchievementClasses;
using discord_clicker.Models.Items.BuildClasses;
using discord_clicker.Models.Items.UpgradeClasses;
using discord_clicker.Services.ItemHandler;

namespace discord_clicker;

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
                options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/");
            });
        
        services.AddAutoMapper(typeof(Startup));
        services.AddMemoryCache();
        
        services.AddTransient<IUserHandler, UserHandler>();
            
        services.AddTransient<IItemHandler<Upgrade, UpgradeViewModel, UpgradeCreateModel>, 
            ItemHandler<Upgrade, UpgradeViewModel, UpgradeCreateModel>>();
        services.Decorate<IItemHandler<Upgrade, UpgradeViewModel, UpgradeCreateModel>, 
            ItemHandlerCachingDecorator<Upgrade, UpgradeViewModel, UpgradeCreateModel>>();
        
        services.AddTransient<IItemHandler<Achievement, AchievementViewModel, AchievementCreateModel>, 
            ItemHandler<Achievement, AchievementViewModel, AchievementCreateModel>>();
        services.Decorate<IItemHandler<Achievement, AchievementViewModel, AchievementCreateModel>, 
            ItemHandlerCachingDecorator<Achievement, AchievementViewModel, AchievementCreateModel>>();
        
        services.AddTransient<IItemHandler<Build, BuildViewModel, BuildCreateModel>, 
            ItemHandler<Build, BuildViewModel, BuildCreateModel>>();
        services.Decorate<IItemHandler<Build, BuildViewModel, BuildCreateModel>, 
            ItemHandlerCachingDecorator<Build, BuildViewModel, BuildCreateModel>>();
            
        services.AddSignalR();
        services.AddSession();
        services.AddControllersWithViews();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseDeveloperExceptionPage();
        app.UseStaticFiles();
        app.UseSession();
        app.UseRouting();
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
            // endpoints.MapHub<ClickerHub>("/clicker");
        });
    }
}