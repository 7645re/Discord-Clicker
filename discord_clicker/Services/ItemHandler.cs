using discord_clicker.Models;
using discord_clicker.ViewModels;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Threading.Tasks;
using System.Linq;
using System;


namespace discord_clicker.Services {
    public class ItemHandler<T, VT, UT> where T : class, IItem<VT> where UT : class, IUserItem<T>, new() {
        private DatabaseContext _db;
        private IMemoryCache _cache;
        private readonly ILogger _logger;
        private readonly IConfiguration _сonfiguration;
        public ItemHandler(DatabaseContext context, IMemoryCache memoryCache, IConfiguration configuration, ILogger<ItemHandler<T, VT, UT>> logger)
        {
            _db = context;
            _logger = logger;
            _cache = memoryCache;
            _сonfiguration = configuration;
        }
        public async Task<List<VT>> GetItemsList(int userId, DbSet<T> items) {
            List<VT> itemsList;
            /** Сhecking for data in the cache */
            bool availabilityСache = _cache.TryGetValue(userId.ToString() + $".{typeof(T).FullName}s", out itemsList);
            if (!availabilityСache)
            {
                itemsList = new List<VT>();
                List<T> itemsListLinks = await items.Where(p => p.Name != null).ToListAsync();
                VT itemModel;
                foreach (T item in itemsListLinks)
                {
                    itemModel = item.ToViewModel();
                    itemsList.Add(itemModel);
                }
                /** Set option to never remove user from cache */
                _cache.Set(userId.ToString() + $".{typeof(T).FullName}s", itemsList, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }
            return itemsList;
        }
        async public Task<Dictionary<bool, string>> BuyItem(int userId, int itemId, decimal money, DbSet<T> itemsContext) {
            User user;
            T item;
            bool availabilityСacheUser = _cache.TryGetValue(userId.ToString()+".WithUserItems", out user);
            bool availabilityСacheItem = _cache.TryGetValue(userId.ToString()+$".{typeof(T).FullName}.{itemId}", out item);

            if (!availabilityСacheUser) {
                _logger.LogInformation("UserModel were not found in the cache and were taken from the database");
                user = await _db.Users.Where(u => u.Id == userId)
                    .Include(u => u.UserBuilds).ThenInclude(up => up.Item)
                    .Include(u => u.UserUpgrades).ThenInclude(uu => uu.Item)
                    .Include(u => u.UserAchievements).ThenInclude(ua => ua.Item)
                    .FirstOrDefaultAsync();
            }
            if (!availabilityСacheItem) {
                item = await itemsContext.Where(i => i.Id == itemId).FirstOrDefaultAsync();
            }
            if (item == null) {
                return new Dictionary<bool, string> {
                    {false, "item doesnt exist"}
                };
            }
            decimal userInterval = Convert.ToDecimal((DateTime.Now - user.LastRequestDate).TotalMilliseconds);
            _logger.LogInformation(userInterval.ToString());
            bool verifyMoney = userInterval*(user.PassiveCoefficient+20*user.ClickCoefficient)/1000>=money;
            if (!verifyMoney) {
                return new Dictionary<bool, string> {
                    {false, "unnormal coint farm"}
                };
            }
            (bool transactionFlag, string message, User user) transactionResult;
            transactionResult = item.Get(user, money);
            if (!transactionResult.transactionFlag) {
                return new Dictionary<bool, string> {
                    {false, transactionResult.message}
                };
            }
            transactionResult.user.LastRequestDate = DateTime.Now;
            user=transactionResult.user;
            _cache.Set(userId.ToString()+".WithUserItems", user, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            _cache.Set(userId.ToString()+$".{typeof(T).FullName}.{itemId}", item, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            return new Dictionary<bool, string> {
                {true, "succes"}
            };
        }
    }
}