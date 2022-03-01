using discord_clicker.Models;
using discord_clicker.ViewModels;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore;
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
            List<BuildModel> buildsList;
            /** Сhecking for data in the cache */
            bool availabilityСache = _cache.TryGetValue(userId.ToString() + ".buildsList", out buildsList);
            if (!availabilityСache)
            {
                buildsList = new List<BuildModel>();
                List<Build> buildsListLinks = await _db.Builds.Where(p => p.Name != null).Include(p => p.UserBuilds).ToListAsync();
                BuildModel buildModel;
                bool presenceRowInTable;
                foreach (Build build in buildsListLinks)
                {
                    buildModel = build.ToBuildModel();
                    presenceRowInTable = build.UserBuilds.Where(ub => ub.UserId == userId && ub.BuildId == build.Id).FirstOrDefault() != null;
                    if (presenceRowInTable) {
                        buildModel.UsersCount = build.UserBuilds.Where(ub => ub.UserId == userId && ub.BuildId == build.Id).First().Count;
                    }
                    buildsList.Add(buildModel);
                }
                /** Set option to never remove user from cache */
                _cache.Set(userId.ToString() + ".buildsList", buildsList, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }
            return buildsList;
        }

        async public Task<Dictionary<string, object>> BuyItem(int userId, int buildId, decimal money)
        {       
            User user; /** Get either from the cache or from the database */
            Build build = null; /** Get either from the buildsList or from the database */
            // bool verifyMoney; /** Could a user earn that much money in a period of time */
            bool enoughMoney; /** Does the user have enough money to buy the ability */
            bool availabilityСache; /** Availability of data in the cache */
            bool presenceRowInTable; /** Has the user bought this ability before. Does it exist in the database */
            uint buyedBuildCount = 0; /** The number of purchased this ability */
            Dictionary<string, object> result = new Dictionary<string, object>();
            List<Build> buildsList = new List<Build>(); /** The list of abilities that exist is taken either from the cache or from the database */
            
            availabilityСache = _cache.TryGetValue(userId.ToString()+".info", out user) && _cache.TryGetValue(userId.ToString() + ".buildsList", out buildsList);

            if (!availabilityСache) {
                /** if the data could not be found in the cache, we turn to the database */
                build = await _db.Builds.Where(p => p.Id == buildId).FirstOrDefaultAsync();
                user = await _db.Users.Include(u => u.UserBuilds).Where(u => u.Id == userId).FirstAsync();
                _logger.LogInformation("User and buildList were not found in the cache and were taken from the database");
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
                _logger.LogInformation($"A user with ID {user.Id} has sent a request to purchase an item with ID {build.Id} that does not exist");
                result.Add("status", "error");
                result.Add("reason", $"The ability with ID {buildId} does not exist");
                return result;
            }

            // verifyMoney = VerifyMoney(user.LastRequestDate, user.Money, money, user.ClickCoefficient,  user.PassiveCoefficient);
            presenceRowInTable = user.UserBuilds.Where(up => up.UserId == userId && up.BuildId == buildId).FirstOrDefault() != null; 
            buyedBuildCount = !presenceRowInTable ? 1 : user.UserBuilds.Where(up => up.UserId == userId && up.BuildId == buildId).First().Count+1;
            enoughMoney = money >= build.Cost*buyedBuildCount;

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
            user.Money=money-build.Cost*(buyedBuildCount == 0 ? 1 : buyedBuildCount);

            // if (_cache.TryGetValue(userId.ToString()+".buildsCount", out buildsCount)) {
            //     buildsCount[buildId]+=1;
            //     _cache.Set(userId.ToString()+".buildsCount", buildsCount, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            // }
            if (!availabilityСache) {
                _cache.Set(userId.ToString()+".info", user, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }

            result.Add("status", "ok");
            result.Add("clickСoefficient", user.ClickCoefficient);
            result.Add("passiveСoefficient", user.PassiveCoefficient);
            result.Add("buyedBuildCount", buyedBuildCount);
            result.Add("cost", build.Cost);
            result.Add("money", user.Money);
            result.Add("buildName", build.Name);
            return result;
        }
    }
}