using System;
using discord_clicker.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using discord_clicker.ViewModels;
using Microsoft.Extensions.Logging;

namespace discord_clicker.Services
{
    public abstract class ItemHandlerCachingDecorator<T, VT> : IItemHandler<T, VT> where T : class, IItem<T, VT>, new()
    {
        private readonly IItemHandler<T, VT> _itemHandler;
        private readonly IMemoryCache _cache;
        private readonly DatabaseContext _db;
        private readonly ILogger _logger;
        
        // key for getting viewmodel items with types (exp: buildModel, upgradeModel, achievementModel) from cache
        private readonly string _itemsVtCacheKey = typeof(VT).Name;

        protected ItemHandlerCachingDecorator(IItemHandler<T, VT> itemHandler, IMemoryCache cache, 
            DatabaseContext db, ILogger<ItemHandlerCachingDecorator<T, VT>> logger)
        {
            _logger = logger;
            _db = db;
            _itemHandler = itemHandler;
            _cache = cache;
        }

        
        
        public async Task<List<VT>> GetItemsList(DbSet<T> itemsContext)
        {
            if (_cache.TryGetValue(_itemsVtCacheKey, out List<VT> itemsList))
            {
                _logger.LogInformation($"{_itemsVtCacheKey}s was taken from cache");
                return itemsList;
            }
            itemsList = await _itemHandler.GetItemsList(itemsContext: itemsContext);
            _cache.Set(_itemsVtCacheKey, itemsList, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            _logger.LogInformation($"{_itemsVtCacheKey}s was taken from database");
            return itemsList;
        }

        public async Task<VT> CreateItem(Dictionary<string, object> parameters, DbSet<T> itemsContext)
        {
            //when creating an object, it will be sent to the user's client and added to the assortment (SignalR)
            return await _itemHandler.CreateItem(parameters: parameters, itemsContext: itemsContext);
        }

        public async Task<Dictionary<bool, string>> BuyItem(int userId, int itemId, decimal money, DbSet<T> itemsContext)
        {
            bool availabilityСacheUser = _cache.TryGetValue(userId.ToString(), out User user);
            bool availabilityСacheItem = _cache.TryGetValue(_itemsTCacheKey, out List<T> items);
            if (!availabilityСacheUser) {
                user = await _db.Users.Where(u => u.Id == userId)
                    .Include(u => u.UserBuilds).ThenInclude(up => up.Item)
                    .Include(u => u.UserUpgrades).ThenInclude(uu => uu.Item)
                    .Include(u => u.UserAchievements).ThenInclude(ua => ua.Item)
                    .FirstOrDefaultAsync();
                    _cache.Set(userId.ToString(), user, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }
            if (!availabilityСacheItem) {
                items = await itemsContext.Where(i => i.Id != null).ToListAsync();
                _cache.Set(userId+$".{typeof(T).FullName}s", items, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }
            return await _itemHandler.BuyItem(userId: userId, itemId: itemId, money: money, itemsContext: itemsContext);
        }
    }
}


