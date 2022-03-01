using discord_clicker.Models;
using discord_clicker.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace discord_clicker.Services {
    class UserHandler : IPersonHandler<UserModel> {
        private UserContext _db;
        private IMemoryCache _cache;
        private readonly ILogger _logger;
        public UserHandler(UserContext context, IMemoryCache memoryCache, ILogger<UserHandler> logger)
        {
            _db = context;
            _logger = logger;
            _cache = memoryCache;
        }
        #nullable enable
        public async Task<UserModel?> GetInfoByName(string nickname) {
            #nullable enable
            User? user;
            UserModel userModel;
            bool availabilityСache = _cache.TryGetValue(nickname.ToString() + ".info", out userModel);
            if (!availabilityСache) {
                user = await _db.Users.Where(u => u.Nickname == nickname).FirstOrDefaultAsync();
                if (user != null) {
                    userModel = user.ToUserModel();
                    _cache.Set(nickname.ToString() + ".info", userModel, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
                }
            }
            return userModel;
        }
        #nullable enable
        public async Task<UserModel?> GetInfoById(int userId) {
            #nullable enable
            User? user;
            UserModel userModel;
            bool availabilityСache = _cache.TryGetValue(userId.ToString() + ".info", out userModel);
            if (!availabilityСache) {
                user = await _db.Users.Where(u => u.Id == userId)
                    .Include(u => u.UserBuilds).ThenInclude(up => up.Build)
                    .Include(u => u.UserUpgrades).ThenInclude(uu => uu.Upgrade)
                    .Include(u => u.UserAchievements).ThenInclude(ua => ua.Achievement)
                    .FirstOrDefaultAsync();
                userModel = user.ToUserModel();
                foreach (UserBuild userBuild in user.UserBuilds) {
                    if (userBuild.Build != null) {
                        userModel.BuyedBuilds.Add(userBuild.Build.Name, userBuild.Count);
                    }
                }
                foreach (UserUpgrade userUpgrade in user.UserUpgrades) {
                    if (userUpgrade.Upgrade != null) {
                        userModel.BuyedUpgrades.Add(userUpgrade.Upgrade.Name, userUpgrade.Count);
                    }
                }
                foreach (UserAchievement userAchievement in user.UserAchievements) {
                    if (userAchievement.Achievement != null) {
                        userModel.Achievements.Add(userAchievement.Achievement.Name, userAchievement.DateOfachievement);
                    }
                }
                _cache.Set(userId.ToString() + ".info", userModel, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }
            return userModel;
        }
        #nullable enable
        public async Task<UserModel?> GetInfoByPass(string nickname, string password) {
            #nullable enable
            User? user;
            UserModel userModel;
            bool availabilityСache = _cache.TryGetValue(nickname.ToString() + ".info", out userModel);
            if (!availabilityСache) {
                user = await _db.Users.Where(u => u.Nickname == nickname  && u.Password == password).FirstOrDefaultAsync();
                if (user != null) {
                    userModel = user.ToUserModel();
                    _cache.Set(nickname.ToString() + ".info", userModel, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
                }
            }
            return userModel;
        }
        public async Task<UserModel> Create(string nickname, string password, decimal money, decimal clickCoefficient, 
            decimal passiveCoefficient) {
            User user = new User { Nickname = nickname, Password = password, Money = money, ClickCoefficient = clickCoefficient, PassiveCoefficient = passiveCoefficient, LastRequestDate=DateTime.Now };
            UserModel userModel = user.ToUserModel();
            _cache.Set(nickname + ".info", userModel, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return userModel;
        }
    }
}