using discord_clicker.Models;
using discord_clicker.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using discord_clicker.Services;


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
        services.AddMemoryCache();
        
        services.AddTransient<IUserHandler, UserHandler>();
        services.Decorate<IUserHandler, UserHandlerCachingDecorator>();
            
        services.AddTransient<IItemHandler<Upgrade, UpgradeModel>, ItemHandler<Upgrade, UpgradeModel>>();
        services.Decorate<IItemHandler<Upgrade, UpgradeModel>, ItemHandlerCachingDecorator<Upgrade, UpgradeModel>>();

        services.AddTransient<IItemHandler<Achievement, AchievementModel>, ItemHandler<Achievement, AchievementModel>>();
        services.Decorate<IItemHandler<Achievement, AchievementModel>, ItemHandlerCachingDecorator<Achievement, AchievementModel>>();

        services.AddTransient<IItemHandler<Build, BuildModel>, ItemHandler<Build, BuildModel>>();
        services.Decorate<IItemHandler<Build, BuildModel>, ItemHandlerCachingDecorator<Build, BuildModel>>();
            
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
            endpoints.MapHub<ClickerHub>("/clicker");
        });
    }
}