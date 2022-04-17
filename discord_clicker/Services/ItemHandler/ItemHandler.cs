using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using discord_clicker.Models;
using discord_clicker.Models.Items;
using discord_clicker.Models.Person;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace discord_clicker.Services.ItemHandler;
//IItemHandler<TItemType, TItemViewModel> 
//where TItemType : class, IItem, new()
public class ItemHandler<TItemType, TItemViewModel> : IItemHandler<TItemType, TItemViewModel> where TItemType : class, IItem, new()
{
    private readonly ILogger _logger;
    private readonly DatabaseContext _db;
    private readonly IMemoryCache _cache;
    private static readonly TItemType StaticItem = new TItemType();
    private readonly IMapper _mapper;

    public ItemHandler(DatabaseContext context, IMemoryCache memoryCache, ILogger<ItemHandler<TItemType, 
        TItemViewModel>> logger, IMapper mapper)
    {
        _mapper = mapper;
        _db = context;
        _logger = logger;
        _cache = memoryCache;
    }

    public async Task<List<TItemViewModel>> GetItemsList(DbSet<TItemType> itemsContext)
    {
        List<TItemType> itemsListLinks = await itemsContext.ToListAsync();  
        List<TItemViewModel> itemsList = new List<TItemViewModel>();
        foreach (TItemType item in itemsListLinks)
        {
            itemsList.Add(_mapper.Map<TItemViewModel>(item));
        }
        return itemsList;
    }
    
    // public async Task<T> CreateItem(Dictionary<string, object> parameters, DbSet<T> itemsContext)
    // {
    //     T item = StaticItem.Create(parameters);
    //     bool itemExist = itemsContext.Any(i => i.Id == (int)parameters["Id"]);
    //     if (!itemExist)
    //     {
    //         itemsContext.Add(item);
    //         await _db.SaveChangesAsync();
    //     }
    //     return item;
    // }
    //
    //
    // public Task<Dictionary<bool, string>> BuyItem(int userId, int itemId, decimal money, DbSet<T> itemsContext)
    // {
    //     User user;
    //     List<T> items;
    //     _cache.TryGetValue(userId, out user);
    //     _cache.TryGetValue($"{typeof(T).Name}", out items);
    //     T item = items.FirstOrDefault(i => i.Id == itemId);
    //
    //     if (item == null)
    //     {
    //         return Task.FromResult(new Dictionary<bool, string>
    //         {
    //             {false, "item doesnt exist"}
    //         });
    //     }
    //
    //     decimal userInterval = Convert.ToDecimal((DateTime.UtcNow - user.LastRequestDate).TotalMilliseconds);
    //     bool verifyMoney = userInterval * (user.PassiveCoefficient + 20 * user.ClickCoefficient) / 1000 >= money;
    //
    //     if (!verifyMoney)
    //     {
    //         return Task.FromResult(new Dictionary<bool, string>
    //         {
    //             {false, "Cheat"}
    //         });
    //     }
    //
    //     (bool transactionFlag, string message, User user) transactionResult = item.Get(user, money);
    //         
    //     if (!transactionResult.transactionFlag)
    //     {
    //         return Task.FromResult(new Dictionary<bool, string>
    //         {
    //             {false, transactionResult.message}
    //         });
    //     }
    //     transactionResult.user.LastRequestDate = DateTime.UtcNow;
    //     _cache.Set(userId, transactionResult.user, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
    //     _cache.Set(userId+".UserModel", transactionResult.user.ToViewModel(), new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
    //     return Task.FromResult(new Dictionary<bool, string>
    //     {
    //         {true, "ok"}
    //     });
    // }
}