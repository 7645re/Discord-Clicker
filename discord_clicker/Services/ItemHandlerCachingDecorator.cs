using System;
using discord_clicker.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace discord_clicker.Services
{
    public class ItemHandlerCachingDecorator<T, VT> : IItemHandler<T, VT> where T : class, IItem<T, VT>, new()
    {
        private readonly IItemHandler<T, VT> _itemHandler;
        private readonly IMemoryCache _cache;
        private readonly DatabaseContext _db;
        private readonly ILogger _logger;
        
        // key for getting viewmodel items with types (exp: buildModel, upgradeModel, achievementModel) from cache
        private readonly string ITEMS_VT_CACHE_KEY = typeof(VT).Name;
        // key for getting items with T types (exp: build, upgrade, achievement) from cache
        private readonly string ITEMS_T_CACHE_KEY = typeof(T).Name;
        
        
        public ItemHandlerCachingDecorator(IItemHandler<T, VT> itemHandler, IMemoryCache cache, 
            DatabaseContext db, ILogger<ItemHandlerCachingDecorator<T, VT>> logger)
        {
            _logger = logger;
            _db = db;
            _itemHandler = itemHandler;
            _cache = cache;
            
            //Load itemsList into cache
            
            
        }

        
        
        public async Task<List<VT>> GetItemsList(int userId, DbSet<T> items)
        {
            bool availabilityСacheTItems = _cache.TryGetValue(ITEMS_T_CACHE_KEY, out List<T> itemsListLinks);
            bool availabilityСacheVtItems = _cache.TryGetValue(ITEMS_VT_CACHE_KEY, out List<VT> itemsList);

            if (availabilityСacheVtItems)
            {
                _logger.LogInformation($"{ITEMS_T_CACHE_KEY} was taken from cache");
                return itemsList;
            }
            itemsList = await _itemHandler.GetItemsList(userId: userId, items: items);
            _cache.Set(ITEMS_VT_CACHE_KEY, itemsList, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            if (!availabilityСacheTItems)
            {
                _logger.LogInformation($"{ITEMS_T_CACHE_KEY} was taken from database");
                itemsListLinks = await items.Where(p => p.Name != null).ToListAsync();
                _cache.Set(ITEMS_T_CACHE_KEY, itemsListLinks, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }
            _logger.LogInformation($"{ITEMS_VT_CACHE_KEY} was taken from database");
            return itemsList;
        }

        public VT CreateItem(Dictionary<string, object> parameters, DbSet<T> itemsContext)
        {
            // when creating an object, you need to write it to the cache (ITEMS_CACHE_LIST)
            return _itemHandler.CreateItem(parameters: parameters, itemsContext: itemsContext);
        }

        public async Task<Dictionary<bool, string>> BuyItem(int userId, int itemId, decimal money,
            DbSet<T> itemsContext)
        {
            bool availabilityСacheUser = _cache.TryGetValue(userId.ToString(), out User user);
            bool availabilityСacheItem = _cache.TryGetValue(ITEMS_T_CACHE_KEY, out List<T> items);
            if (!availabilityСacheUser) {
                // _logger.LogInformation("user taked from db");
                user = await _db.Users.Where(u => u.Id == userId)
                    .Include(u => u.UserBuilds).ThenInclude(up => up.Item)
                    .Include(u => u.UserUpgrades).ThenInclude(uu => uu.Item)
                    .Include(u => u.UserAchievements).ThenInclude(ua => ua.Item)
                    .FirstOrDefaultAsync();
                    _cache.Set(userId.ToString(), user, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }
            if (!availabilityСacheItem) {
                // _logger.LogInformation(ITEMS_T_CACHE_KEY + "taken from db");
                items = await itemsContext.Where(i => i.Id != null).ToListAsync();
                _cache.Set(userId+$".{typeof(T).FullName}s", items, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }
            return await _itemHandler.BuyItem(userId: userId, itemId: itemId, money: money, itemsContext: itemsContext);
        }
    }
}


