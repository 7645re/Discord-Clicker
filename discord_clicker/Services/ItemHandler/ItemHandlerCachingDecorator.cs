using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using discord_clicker.Models.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;


namespace discord_clicker.Services.ItemHandler;

// ReSharper disable once InconsistentNaming
public class ItemHandlerCachingDecorator<TItem, TItemViewModel, TItemCreateModel> : IItemHandler<TItem, TItemViewModel, TItemCreateModel>
    where TItem : class, IItem<TItem, TItemCreateModel>, new()
    where TItemCreateModel : IItemCreateModel
{
    private readonly IItemHandler<TItem, TItemViewModel, TItemCreateModel> _itemHandler;
    private readonly IMemoryCache _cache;

    public ItemHandlerCachingDecorator(IItemHandler<TItem, TItemViewModel, TItemCreateModel> itemHandler, 
        IMemoryCache cache)
    {
        _itemHandler = itemHandler;
        _cache = cache;
    }

    public async Task<List<TItemViewModel>> GetItemsList(DbSet<TItem> itemsContext)
    {
        _cache.TryGetValue(typeof(TItem).Name, out List<TItemViewModel> cachingItems);
        if (cachingItems != null)
        {
            return cachingItems;
        }
        cachingItems = await _itemHandler.GetItemsList(itemsContext);
        _cache.Set(typeof(TItem).Name, cachingItems, TimeSpan.FromSeconds(1));
        return cachingItems;
    }
    public async Task<TItemViewModel> CreateItem(TItemCreateModel itemCreateModel, DbSet<TItem> itemContext)
    {
        return await _itemHandler.CreateItem(itemCreateModel, itemContext);
    }

    public async Task<Dictionary<string, object>> BuyItem(int userId, int itemId, long money, DbSet<TItem> itemsContext)
    {
        return await _itemHandler.BuyItem(userId, itemId, money, itemsContext);
    }
    public async Task DeleteItems(DbSet<TItem> itemsContext)
    {
        await _itemHandler.DeleteItems(itemsContext);
    }

    public async Task DeleteItem(DbSet<TItem> itemsContext, int id)
    {
        await _itemHandler.DeleteItem(itemsContext, id);
    } 
}