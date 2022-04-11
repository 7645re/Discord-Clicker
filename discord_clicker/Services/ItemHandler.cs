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


namespace discord_clicker.Services;

/// <summary>
/// A class designed to interact with various items, to buy them and display the assortment
/// </summary>
/// <typeparam name="T"> Type of item </typeparam>
/// <typeparam name="VT">  Type of Viewitem for mapping item </typeparam>
public class ItemHandler<T, VT> : IItemHandler<T, VT> where T : class, IItem<T, VT>, new()
{
    private readonly DatabaseContext _db;
    private readonly IMemoryCache _cache;
    private readonly ILogger _logger;
    private static readonly T StaticItem = new T();

    public ItemHandler(DatabaseContext context, IMemoryCache memoryCache, ILogger<ItemHandler<T, VT>> logger)
    {
        _db = context;
        _logger = logger;
        _cache = memoryCache;
    }

    public async Task<List<VT>> GetItemsList(DbSet<T> itemsContext)
    {
        List<T> itemsListLinks = await itemsContext.ToListAsync();
        List<VT> itemsList = new List<VT>();
        foreach (T item in itemsListLinks)
        {
            itemsList.Add(item.ToViewModel());
        }
        return itemsList;
    }

    public async Task<VT> CreateItem(Dictionary<string, object> parameters, DbSet<T> itemsContext)
    {
        T item = StaticItem.Create(parameters);
        bool itemExist = itemsContext.Any(i => i.Id == (int)parameters["Id"]);
        if (!itemExist)
        {
            string itemsCharact = "";
            foreach (KeyValuePair<string, object> charact in parameters)
            {
                itemsCharact += $"{charact.Key}: {charact.Value} ";
            }

            itemsContext.Add(item);
            _logger.LogInformation($"An {typeof(T).Name} object with the characteristics " +
                                   itemsCharact +
                                   $"was created and recorded in the database");
            await _db.SaveChangesAsync();
        }
        return item.ToViewModel();
    }


    public async Task<Dictionary<bool, string>> BuyItem(int userId, int itemId, decimal money, DbSet<T> itemsContext)
    {
        User user;
        List<T> items;
        _cache.TryGetValue(userId, out user);
        _cache.TryGetValue($"{typeof(T).Name}", out items);
        T item = items.FirstOrDefault(i => i.Id == itemId);

        if (item == null)
        {
            return new Dictionary<bool, string>
            {
                {false, "item doesnt exist"}
            };
        }

        decimal userInterval = Convert.ToDecimal((DateTime.UtcNow - user.LastRequestDate).TotalMilliseconds);
        bool verifyMoney = userInterval * (user.PassiveCoefficient + 20 * user.ClickCoefficient) / 1000 >= money;

        if (!verifyMoney)
        {
            return new Dictionary<bool, string>
            {
                {false, "Cheat"}
            };
        }

        (bool transactionFlag, string message, User user) transactionResult = item.Get(user, money);
            
        if (!transactionResult.transactionFlag)
        {
            return new Dictionary<bool, string>
            {
                {false, transactionResult.message}
            };
        }
        transactionResult.user.LastRequestDate = DateTime.UtcNow;
        _cache.Set(userId, transactionResult.user, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
        _cache.Set(userId+".UserModel", transactionResult.user.ToViewModel(), new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
        return new Dictionary<bool, string>
        {
            {true, "ok"}
        };
    }
}