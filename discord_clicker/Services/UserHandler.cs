using discord_clicker.Models;
using discord_clicker.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore.Query;


namespace discord_clicker.Services;

public class UserHandler : IUserHandler
{
    private readonly DatabaseContext _db;
    private readonly IMemoryCache _cache;
    private readonly ILogger _logger;

    public UserHandler(DatabaseContext context, IMemoryCache memoryCache, ILogger<UserHandler> logger)
    {
        _db = context;
        _logger = logger;
        _cache = memoryCache;
    }

    /// <summary> Function to check whether a user with such a nickname exists </summary>
    public async Task<bool> ExistCheck(string nickname)
    {
        #nullable enable
        User? user = await _db.Users.Where(u => u.Nickname == nickname).FirstOrDefaultAsync();
        return (user != null);
    }
    #nullable enable
    public async Task<UserModel?> GetUser(int? userId = null, string? name = null, string? password = null)
    {
        #nullable enable
        User? user;
        UserModel? userModel = null;
        // _ = userId == null && name != null && password != null
        //     ? _ =_cache.TryGetValue(name, out user)
        //     : _ =_cache.TryGetValue(userId, out user);
        _ = _cache.TryGetValue(userId, out user);
        if (user == null)
        {
            return userModel;
        }

        userModel = user.ToViewModel();
        foreach (UserBuild userBuild in user.UserBuilds)
        {
            if (userBuild.Item != null)
            {
                userModel.Builds.Add(userBuild.Item.Id,
                    new Dictionary<string, object>
                    {
                        {"ItemName", userBuild.Item.Name}, {"ItemCount", userBuild.Count},
                        {"PassiveCoefficient", userBuild.PassiveCoefficient}
                    });
            }
        }

        foreach (UserUpgrade userUpgrade in user.UserUpgrades)
        {
            if (userUpgrade.Item != null)
            {
                userModel.Upgrades.Add(userUpgrade.Item.Name, userUpgrade.Count);
            }
        }

        foreach (UserAchievement userAchievement in user.UserAchievements)
        {
            if (userAchievement.Item != null)
            {
                userModel.Achievements.Add(userAchievement.Item.Name, userAchievement.DateOfAchievement);
            }
        }
        return userModel;
    }

    public async Task<User> Create(string nickname, string password, decimal money, decimal clickCoefficient,
        decimal passiveCoefficient, DateTime playStartDate)
    {
        Role userRole = await _db.Roles.FirstAsync(r => r.Name == "user");
        User user = new User
        {
            Nickname = nickname, Password = password, Money = money, ClickCoefficient = clickCoefficient,
            PassiveCoefficient = passiveCoefficient, LastRequestDate = DateTime.UtcNow,
            PlayStartDate = playStartDate, Role = userRole
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }
}