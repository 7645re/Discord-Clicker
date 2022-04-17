using System.Collections.Generic;
using System.Threading.Tasks;
using discord_clicker.Models.Items;
using Microsoft.EntityFrameworkCore;

namespace discord_clicker.Services.ItemHandler;

public interface IItemHandler<TItemType, TItemViewType> where TItemType : class, IItem, new()
{
    public Task<List<TItemViewType>> GetItemsList(DbSet<TItemType> itemsContext);
    // public Task<TItemViewType> CreateItem();
    // public Task<Dictionary<bool, string>> BuyItem(int userId, int itemId, decimal money, 
    //     DbSet<TItemType> itemsContext);
}