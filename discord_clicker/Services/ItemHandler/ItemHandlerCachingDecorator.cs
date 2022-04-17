// using System.Collections.Generic;
// using System.Threading.Tasks;
// using discord_clicker.Models;
// using discord_clicker.Models.Items;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Caching.Memory;
//
//
// namespace discord_clicker.Services.ItemHandler;
//
// // ReSharper disable once InconsistentNaming
// public class ItemHandlerCachingDecorator<T, VT> : IItemHandler<T, VT> where T : class, IItem<T, VT>, new()
// {
//     private readonly IItemHandler<T, VT> _itemHandler;
//     private readonly IMemoryCache _cache;
//
//     private readonly string _itemsVtCacheKey = typeof(T).Name;
//
//     public ItemHandlerCachingDecorator(IItemHandler<T, VT> itemHandler, IMemoryCache cache)
//     {
//         _itemHandler = itemHandler;
//         _cache = cache;
//     }
//
//     public async Task<List<VT>> GetItemsList(DbSet<T> itemsContext)
//     {
//         if (_cache.TryGetValue(_itemsVtCacheKey, out List<VT> itemsList)) return itemsList;
//         itemsList = await _itemHandler.GetItemsList(itemsContext: itemsContext);
//         _cache.Set(_itemsVtCacheKey, itemsList, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
//         return itemsList;
//     }
//
//     public async Task<T> CreateItem(Dictionary<string, object> parameters, DbSet<T> itemsContext)
//     {
//         return await _itemHandler.CreateItem(parameters: parameters, itemsContext: itemsContext);
//     }
//
//     public async Task<Dictionary<bool, string>> BuyItem(int userId, int itemId, decimal money, DbSet<T> itemsContext)
//     {
//         return await _itemHandler.BuyItem(userId: userId, itemId: itemId, money: money, itemsContext: itemsContext);
//     }
// }