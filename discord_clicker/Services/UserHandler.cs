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


namespace discord_clicker.Services
{
    public class UserHandler
    {
        private DatabaseContext _db;
        private IMemoryCache _cache;
        private ILogger _logger;

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
            bool availabilityСache = _cache.TryGetValue(userId.ToString(), out user);
            if (!availabilityСache)
            {
                _logger.LogInformation("UserModel were not found in the cache and were taken from the database");
                if (userId != null)
                {
                    user = await _db.Users.Where(u => u.Id == userId)
                        .Include(u => u.UserBuilds).ThenInclude(up => up.Item)
                        .Include(u => u.UserUpgrades).ThenInclude(uu => uu.Item)
                        .Include(u => u.UserAchievements).ThenInclude(ua => ua.Item)
                        .FirstOrDefaultAsync();
                }

                if (name != null && password != null)
                {
                    user = await _db.Users.Where(u => u.Nickname == name && u.Password == password)
                        .Include(u => u.UserBuilds).ThenInclude(up => up.Item)
                        .Include(u => u.UserUpgrades).ThenInclude(uu => uu.Item)
                        .Include(u => u.UserAchievements).ThenInclude(ua => ua.Item)
                        .FirstOrDefaultAsync();
                }
            }

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
                    userModel.Achievements.Add(userAchievement.Item.Name, userAchievement.DateOfachievement);
                }
            }

            if (!availabilityСache)
            {
                _cache.Set(userId.ToString(), user,
                    new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }

            return userModel;
        }

        public async Task<User> Create(string nickname, string password, decimal money, decimal clickCoefficient,
            decimal passiveCoefficient, DateTime playStartDate)
        {
            User user = new User
            {
                Nickname = nickname, Password = password, Money = money, ClickCoefficient = clickCoefficient,
                PassiveCoefficient = passiveCoefficient, LastRequestDate = DateTime.UtcNow,
                PlayStartDate = playStartDate
            };
            _cache.Set(user.Id.ToString(), user, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }
    }
}