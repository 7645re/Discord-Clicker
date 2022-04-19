using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using discord_clicker.Models;
using discord_clicker.Models.Items;
using Microsoft.EntityFrameworkCore;

namespace discord_clicker.Services.ItemHandler;

public class ItemHandler<TItem, TItemViewModel, TItemCreateModel> :
    IItemHandler<TItem, TItemViewModel, TItemCreateModel>
    where TItem : class, IItem<TItem, TItemCreateModel>, new()
    where TItemCreateModel : IItemCreateModel

{
    private readonly DatabaseContext _db;
    private static readonly TItem StaticItem = new ();
    private readonly IMapper _mapper;

    public ItemHandler(DatabaseContext context, IMapper mapper)
    {
        _mapper = mapper;
        _db = context;
    }

    public async Task<List<TItemViewModel>> GetItemsList(DbSet<TItem> itemsContext)
    {
        List<TItem> itemsListLinks = await itemsContext.ToListAsync();
        List<TItemViewModel> itemsList = new List<TItemViewModel>();
        foreach (TItem item in itemsListLinks)
        {
            itemsList.Add(_mapper.Map<TItemViewModel>(item));
        }

        return itemsList;
    }

    public async Task<TItemViewModel> CreateItem(TItemCreateModel itemCreateModel,
        DbSet<TItem> itemContext)
    {
        TItem item = _mapper.Map<TItem>(itemCreateModel);
        await itemContext.AddAsync(item);
        await _db.SaveChangesAsync();
        return _mapper.Map<TItemViewModel>(item);
    }

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