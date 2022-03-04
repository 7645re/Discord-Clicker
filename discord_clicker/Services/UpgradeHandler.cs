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
    public class UpgradeHandler : IItemHandler<UpgradeModel> {
        private UserContext _db;
        private IMemoryCache _cache;
        private readonly ILogger _logger;
        private readonly IConfiguration _сonfiguration;
        public UpgradeHandler(UserContext context, IMemoryCache memoryCache, IConfiguration configuration, ILogger<UpgradeHandler> logger)
        {
            _db = context;
            _logger = logger;
            _cache = memoryCache;
            _сonfiguration = configuration;
        }
        public async Task<List<UpgradeModel>> GetItemsList(int userId) {
            List<UpgradeModel> Upgrades;
            /** Сhecking for data in the cache */
            bool availabilityСache = _cache.TryGetValue(userId.ToString() + ".Upgrades", out Upgrades);
            if (!availabilityСache)
            {
                Upgrades = new List<UpgradeModel>();
                List<Upgrade> upgradesListLinks = await _db.Upgrades.Where(p => p.Name != null).ToListAsync();
                UpgradeModel upgradeModel;
                foreach (Upgrade upgrade in upgradesListLinks)
                {
                    upgradeModel = upgrade.ToUpgradeModel();
                    Upgrades.Add(upgradeModel);
                }
                /** Set option to never remove user from cache */
                _cache.Set(userId.ToString() + ".Upgrades", Upgrades, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }
            return Upgrades;
        }

        async public Task<Dictionary<string, object>> GetItem(int userId, int upgradeId, decimal money)
        {       
            User user; /** Get either from the cache or from the database */
            Upgrade upgrade = null; /** Get either from the upgradesList or from the database */
            // bool verifyMoney; /** Could a user earn that much money in a period of time */
            bool enoughMoney; /** Does the user have enough money to buy the ability */
            bool availabilityСache; /** Availability of data in the cache */
            bool presenceRowInTable; /** Has the user bought this ability before. Does it exist in the database */
            uint count = 0; /** The number of purchased this ability */
            Dictionary<string, object> result = new Dictionary<string, object>();
            List<Upgrade> upgradesList = new List<Upgrade>(); /** The list of abilities that exist is taken either from the cache or from the database */
            UserModel userModel;

            availabilityСache = _cache.TryGetValue(userId.ToString(), out user) && _cache.TryGetValue(userId.ToString() + ".Upgrades", out upgradesList);

            if (!availabilityСache) {
                /** if the data could not be found in the cache, we turn to the database */
                upgrade = await _db.Upgrades.Where(p => p.Id == upgradeId).FirstOrDefaultAsync();
                user = await _db.Users.Include(u => u.UserUpgrades).Where(u => u.Id == userId).FirstAsync();
                _logger.LogInformation("User and Upgrades were not found in the cache and were taken from the database");
            }
            else {
                upgrade = upgradesList.Where(p => p.Id == upgradeId).FirstOrDefault();
            }

            if (money == 0) {
                _logger.LogInformation($"A user with ID {user.Id} sent a request to purchase an item with ID {upgrade.Id} with 0 money");
                result.Add("status", "error");
                result.Add("reason", "When sending requests, the user has 0 coins");
                return result;
            }

            if (upgrade == null) {
                _logger.LogInformation($"A user with ID {user.Id} has sent a request to purchase an item with ID {upgradeId} that does not exist");
                result.Add("status", "error");
                result.Add("reason", $"The ability with ID {upgradeId} does not exist");
                return result;
            }

            // verifyMoney = VerifyMoney(user.LastRequestDate, user.Money, money, user.ClickCoefficient,  user.PassiveCoefficient);
            presenceRowInTable = user.UserUpgrades.Where(up => up.UserId == userId && up.UpgradeId == upgradeId).FirstOrDefault() != null; 
            count = !presenceRowInTable ? 1 : user.UserUpgrades.Where(up => up.UserId == userId && up.UpgradeId == upgradeId).First().Count+1;
            enoughMoney = money >= upgrade.Cost*count;

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
                user.UserUpgrades.Add(new UserUpgrade { UserId=userId, UpgradeId=upgrade.Id, Count=1});
            }
            else {
                user.UserUpgrades.Where(up => up.UserId == userId && up.UpgradeId == upgradeId).First().Count+=1;
            }

            user.Upgrades.Add(upgrade);
            user.LastRequestDate = DateTime.Now;
            // user.PassiveCoefficient+=upgrade.PassiveCoefficient;
            user.Money=money-upgrade.Cost*(count == 0 ? 1 : count);

            if (!availabilityСache) {
                upgradesList = await _db.Upgrades.Where(b => b.Id != 0).ToListAsync();
                _cache.Set(userId.ToString(), user, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
                _cache.Set(userId.ToString()+".Upgrades", upgradesList, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }

            availabilityСache = _cache.TryGetValue(userId.ToString()+".Model", out userModel);

            if (availabilityСache) {
                if (userModel.Upgrades.TryGetValue(upgrade.Name, out _)) {
                    userModel.Upgrades[upgrade.Name] = count;
                }
                else {
                    userModel.Upgrades.Add(upgrade.Name, count);
                }
                _cache.Set(userId.ToString()+".Model", userModel, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }

            result.Add("status", "ok");
            result.Add("clickСoefficient", user.ClickCoefficient);
            result.Add("passiveСoefficient", user.PassiveCoefficient);
            result.Add("count", count);
            result.Add("cost", upgrade.Cost);
            result.Add("money", user.Money);
            result.Add("name", upgrade.Name);

            return result;
        }
    }
}