using discord_clicker.Models;
using discord_clicker.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace discord_clicker.Services {
    class UserHandler : IPersonHandler<User, UserModel> {
        private UserContext _db;
        private IMemoryCache _cache;
        private readonly ILogger _logger;
        public UserHandler(UserContext context, IMemoryCache memoryCache, ILogger<UserHandler> logger)
        {
            _db = context;
            _logger = logger;
            _cache = memoryCache;
        }
        /// <summary> The method is intended only for the registration stage, in all other cases use the more complete GetInfoById function</summary>
        #nullable enable
        public async Task<User?> GetInfoByName(string nickname) {
            #nullable enable
            User? user;
            bool availabilityСache = _cache.TryGetValue(nickname.ToString(), out user);
            if (!availabilityСache) {
                user = await _db.Users.Where(u => u.Nickname == nickname).FirstOrDefaultAsync();
            }
            if (user != null) {
                _cache.Set(nickname.ToString(), user, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }
            return user;
        }
        #nullable enable
        public async Task<UserModel?> GetFullInfoById(int userId) {
            #nullable enable
            User? user;
            UserModel? userModel = null;
            bool availabilityСache = _cache.TryGetValue(userId.ToString()+".Model", out userModel);
            if (!availabilityСache) {
                _logger.LogInformation("UserModel were not found in the cache and were taken from the database");
                user = await _db.Users.Where(u => u.Id == userId)
                    .Include(u => u.UserBuilds).ThenInclude(up => up.Build)
                    .Include(u => u.UserUpgrades).ThenInclude(uu => uu.Upgrade)
                    .Include(u => u.UserAchievements).ThenInclude(ua => ua.Achievement)
                    .FirstOrDefaultAsync();
                userModel = user.ToUserModel();
                foreach (UserBuild userBuild in user.UserBuilds) {
                    if (userBuild.Build != null) {
                        userModel.Builds.Add(userBuild.Build.Name, userBuild.Count);
                    }
                }
                foreach (UserUpgrade userUpgrade in user.UserUpgrades) {
                    if (userUpgrade.Upgrade != null) {
                        userModel.Upgrades.Add(userUpgrade.Upgrade.Name, userUpgrade.Count);
                    }
                }
                foreach (UserAchievement userAchievement in user.UserAchievements) {
                    if (userAchievement.Achievement != null) {
                        userModel.Achievements.Add(userAchievement.Achievement.Name, userAchievement.DateOfachievement);
                    }
                }
                _cache.Set(userId.ToString()+".Model", userModel, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }
            return userModel;
        }
        /// <summary> The method is intended only for the login stage, in all other cases use the more complete GetInfoById function</summary>
        #nullable enable
        public async Task<User?> GetInfoByPass(string nickname, string password) {
            #nullable enable
            User? user;
            bool availabilityСache = _cache.TryGetValue(nickname.ToString(), out user);
            if (!availabilityСache) {
                user = await _db.Users.Where(u => u.Nickname == nickname  && u.Password == password).FirstOrDefaultAsync();
            }
            if (user != null) {
                _cache.Set(nickname.ToString(), user, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }
            return user;
        }
        /// <summary> Method for creating a user </summary>
        public async Task<User> Create(string nickname, string password, decimal money, decimal clickCoefficient, 
            decimal passiveCoefficient) {
            User user = new User { Nickname = nickname, Password = password, Money = money, ClickCoefficient = clickCoefficient, PassiveCoefficient = passiveCoefficient, LastRequestDate=DateTime.Now };
            _cache.Set(nickname, user, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }
    }
}