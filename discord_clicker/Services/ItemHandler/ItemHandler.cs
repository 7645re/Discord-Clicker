using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using discord_clicker.Models;
using discord_clicker.Models.Items;
using discord_clicker.Models.Person;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Numerics;
using System.Xml.XPath;
using discord_clicker.Models.Items.AchievementClasses;
using Microsoft.EntityFrameworkCore;

namespace discord_clicker.Services.ItemHandler;

public class ItemHandler<TItem, TItemViewModel, TItemCreateModel> :
    IItemHandler<TItem, TItemViewModel, TItemCreateModel>
    where TItem : class, IItem<TItem, TItemCreateModel>, new()
    where TItemCreateModel : IItemCreateModel

{
    private readonly DatabaseContext _db;
    private readonly IMapper _mapper;

    public ItemHandler(DatabaseContext context, IMapper mapper)
    {
        _mapper = mapper;
        _db = context;
    }
    public async Task<List<TItemViewModel>> GetItemsList(DbSet<TItem> itemsContext)
    {
        List<TItem> itemsListLinks = await itemsContext.OrderBy(i => i.Id).ToListAsync();
        List<TItemViewModel> itemsList = new List<TItemViewModel>();
        foreach (TItem item in itemsListLinks)
        {
            itemsList.Add(_mapper.Map<TItemViewModel>(item));
        }

        return itemsList;
    }

    public async Task DeleteItems(DbSet<TItem> itemsContext)
    {
        itemsContext.RemoveRange(itemsContext.Where(i => i.Id != null));
        await _db.SaveChangesAsync();
    }

    public async Task<TItemViewModel> CreateItem(TItemCreateModel itemCreateModel,
        DbSet<TItem> itemContext)
    {
        TItem item = _mapper.Map<TItem>(itemCreateModel);
        await itemContext.AddAsync(item);
        await _db.SaveChangesAsync();
        return _mapper.Map<TItemViewModel>(item);
    }

    public async Task DeleteItem(DbSet<TItem> itemsContext, int id)
    {
        TItem item = await itemsContext.Where(i => i.Id == id).FirstAsync();
        itemsContext.Remove(item);
        await _db.SaveChangesAsync();
    }

    public async Task<Dictionary<string, object>> BuyItem(int userId, int itemId, long money,
        DbSet<TItem> itemsContext)
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        #nullable enable
        User? user = await _db.Users.Where(u => u.Id == userId)
            .Include(u => u.UserBuilds)
            .Include(u => u.UserUpgrades)
            .Include(u => u.UserAchievements)
            .FirstOrDefaultAsync();
        TItem? item = await itemsContext.FirstOrDefaultAsync(i => i.Id == itemId);
        if (item == null)
        {
            result.Add("status", "error");
            result.Add("reason", "item doesnt exist");
            return result;
        }

        long diffMoney = money - user.Money;
        if (diffMoney < 0)
        {
            result.Add("status", "error");
            result.Add("reason", "non-fixed debiting of funds");
            return result;
        }

        decimal userInterval = Convert.ToDecimal((DateTime.UtcNow - user.LastRequestDate).TotalMilliseconds);
        bool verifyMoney = userInterval * (user.PassiveCoefficient + 20 * user.ClickCoefficient) / 1000 >= diffMoney;

        if (!verifyMoney)
        {
            result.Add("status", "error");
            result.Add("reason", "cheat");
            return result;
        }

        (bool transactionFlag, string message, User user) transactionResult = item.Get(user, money, _db.Achievements);

        if (!transactionResult.transactionFlag)
        {
            result.Add("status", "error");
            result.Add("reason", transactionResult.message);
            return result;
        }
        transactionResult.user.LastRequestDate = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        result.Add("status", "ok");
        result.Add("user", _mapper.Map<UserViewModel>(user));
        return result;
    }
    
}