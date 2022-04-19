using System.Collections.Generic;
using System.Threading.Tasks;
using discord_clicker.Models.Items;
using Microsoft.EntityFrameworkCore;

namespace discord_clicker.Services.ItemHandler;

public interface IItemHandler<TItemType, TItemViewType, TItemCreateType> where TItemType : 
    class, IItem<TItemType, TItemCreateType>, new() where TItemCreateType : IItemCreateModel
{
    public Task<List<TItemViewType>> GetItemsList(DbSet<TItemType> itemsContext);
    public Task<TItemViewType> CreateItem(TItemCreateType itemCreateModel,
        DbSet<TItemType> itemContext);
}