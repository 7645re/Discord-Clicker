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
        List<TItem> itemsListLinks = await itemsContext.ToListAsync();
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
    
    public async Task<Dictionary<bool, string>> BuyItem(int userId, int itemId, decimal money,
        DbSet<TItem> itemsContext)
    {
        Dictionary<bool, string> result = new Dictionary<bool, string>();
        #nullable enable
        User? user = await _db.Users.Where(u => u.Id == userId)
            .Include(u => u.UserBuilds)
            .Include(u => u.UserUpgrades)
            .Include(u => u.UserAchievements)
            .FirstOrDefaultAsync();
        TItem? item = await itemsContext.FirstOrDefaultAsync(i => i.Id == itemId);
        if (item == null)
        {
            result.Add(false, "item doesnt exist");
            return result;
        }

        decimal userInterval = Convert.ToDecimal((DateTime.UtcNow - user.LastRequestDate).TotalMilliseconds);
        bool verifyMoney = userInterval * (user.PassiveCoefficient + 20 * user.ClickCoefficient) / 1000 >= money;

        if (!verifyMoney)
        {
            result.Add(false, "chear");
            return result;
        }

        (bool transactionFlag, string message, User user) transactionResult = item.Get(user, money, _db.Achievements);

        if (!transactionResult.transactionFlag)
        {
            result.Add(false, transactionResult.message);
            return result;
        }
        transactionResult.user.LastRequestDate = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        result.Add(true, "ok");
        return result;
    }
    
}