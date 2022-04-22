using System.Collections.Generic;
using System.Threading.Tasks;
using discord_clicker.Models.Items;
using Microsoft.EntityFrameworkCore;

namespace discord_clicker.Services.ItemHandler;

public interface IItemHandler<TItem, TItemViewModel, TItemCreateModel> where TItem : 
    class, IItem<TItem, TItemCreateModel>, new() where TItemCreateModel : IItemCreateModel
{
    public Task<List<TItemViewModel>> GetItemsList(DbSet<TItem> itemsContext);
    public Task<TItemViewModel> CreateItem(TItemCreateModel itemCreateModel,
        DbSet<TItem> itemContext);
    public Task<Dictionary<bool, string>> BuyItem(int userId, int itemId, decimal money,
        DbSet<TItem> itemsContext);
}