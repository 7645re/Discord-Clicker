using discord_clicker.Models;
using discord_clicker.ViewModels;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;


namespace discord_clicker.Services {
    public class BuildHandler : IItemHandler<BuildModel> {
        private UserContext _db;
        private IMemoryCache _cache;
        private readonly ILogger _logger;
        private readonly IConfiguration _сonfiguration;
        public BuildHandler(UserContext context, IMemoryCache memoryCache, IConfiguration configuration, ILogger<BuildHandler> logger)
        {
            _db = context;
            _logger = logger;
            _cache = memoryCache;
            _сonfiguration = configuration;
        }
        public async Task<List<BuildModel>> GetItemsList(int userId) {
            List<BuildModel> Builds;
            /** Сhecking for data in the cache */
            bool availabilityСache = _cache.TryGetValue(userId.ToString() + ".Builds", out Builds);
            if (!availabilityСache)
            {
                Builds = new List<BuildModel>();
                List<Build> buildsListLinks = await _db.Builds.Where(p => p.Name != null).ToListAsync();
                BuildModel buildModel;
                foreach (Build build in buildsListLinks)
                {
                    buildModel = build.ToBuildModel();
                    Builds.Add(buildModel);
                }
                /** Set option to never remove user from cache */
                _cache.Set(userId.ToString() + ".Builds", Builds, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }
            return Builds;
        }

        async public Task<Dictionary<string, object>> GetItem(int userId, int buildId, decimal money)
        {       
            User user; /** Get either from the cache or from the database */
            Build build = null; /** Get either from the buildsList or from the database */
            // bool verifyMoney; /** Could a user earn that much money in a period of time */
            bool enoughMoney; /** Does the user have enough money to buy the ability */
            bool availabilityСache; /** Availability of data in the cache */
            bool presenceRowInTable; /** Has the user bought this ability before. Does it exist in the database */
            uint count = 0; /** The number of purchased this ability */
            Dictionary<string, object> result = new Dictionary<string, object>();
            List<Build> buildsList = new List<Build>(); /** The list of abilities that exist is taken either from the cache or from the database */
            UserModel userModel;

            availabilityСache = _cache.TryGetValue(userId.ToString(), out user) && _cache.TryGetValue(userId.ToString() + ".Builds", out buildsList);

            if (!availabilityСache) {
                /** if the data could not be found in the cache, we turn to the database */
                build = await _db.Builds.Where(p => p.Id == buildId).FirstOrDefaultAsync();
                user = await _db.Users.Include(u => u.UserBuilds).Where(u => u.Id == userId).FirstAsync();
                _logger.LogInformation("User and Builds were not found in the cache and were taken from the database");
            }
            else {
                build = buildsList.Where(p => p.Id == buildId).FirstOrDefault();
            }

            if (money == 0) {
                _logger.LogInformation($"A user with ID {user.Id} sent a request to purchase an item with ID {build.Id} with 0 money");
                result.Add("status", "error");
                result.Add("reason", "When sending requests, the user has 0 coins");
                return result;
            }

            if (build == null) {
                _logger.LogInformation($"A user with ID {user.Id} has sent a request to purchase an item with ID {buildId} that does not exist");
                result.Add("status", "error");
                result.Add("reason", $"The ability with ID {buildId} does not exist");
                return result;
            }

            // verifyMoney = VerifyMoney(user.LastRequestDate, user.Money, money, user.ClickCoefficient,  user.PassiveCoefficient);
            presenceRowInTable = user.UserBuilds.Where(up => up.UserId == userId && up.BuildId == buildId).FirstOrDefault() != null; 
            count = !presenceRowInTable ? 1 : user.UserBuilds.Where(up => up.UserId == userId && up.BuildId == buildId).First().Count+1;
            enoughMoney = money >= build.Cost*count;

            if (!enoughMoney) {
                result.Add("status", "error");
                result.Add("reason", "There are not enough funds to buy the ability");
                return result;
            }

            // if (!verifyMoney) {
            //     return Json(new {result="cheat", reason=$"You could not earn {money-user.Money} coins in a time interval of {(DateTime.Now-user.LastRequestDate).TotalMilliseconds}s", money=Convert.ToDecimal((DateTime.Now-user.LastRequestDate).TotalMilliseconds/1000*user.PassiveCoefficient),
            //     user.ClickCoefficient, user.PassiveCoefficient});
            // }
            if (!presenceRowInTable) {
                user.UserBuilds.Add(new UserBuild { UserId=userId, BuildId=build.Id, Count=1});
            }
            else {
                user.UserBuilds.Where(up => up.UserId == userId && up.BuildId == buildId).First().Count+=1;
            }

            user.Builds.Add(build);
            user.LastRequestDate = DateTime.Now;
            user.PassiveCoefficient+=build.PassiveCoefficient;
            user.Money=money-build.Cost*(count == 0 ? 1 : count);

            if (!availabilityСache) {
                buildsList = await _db.Builds.Where(b => b.Id != 0).ToListAsync();
                _cache.Set(userId.ToString(), user, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
                _cache.Set(userId.ToString()+".Builds", buildsList, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }

            availabilityСache = _cache.TryGetValue(userId.ToString()+".Model", out userModel);

            if (availabilityСache) {
                if (userModel.Builds.TryGetValue(build.Name, out _)) {
                    userModel.Builds[build.Name] = count;
                }
                else {
                    userModel.Builds.Add(build.Name, count);
                }
                _cache.Set(userId.ToString()+".Model", userModel, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }

            result.Add("status", "ok");
            result.Add("clickСoefficient", user.ClickCoefficient);
            result.Add("passiveСoefficient", user.PassiveCoefficient);
            result.Add("count", count);
            result.Add("cost", build.Cost);
            result.Add("money", user.Money);
            result.Add("name", build.Name);

            return result;
        }
    }
}