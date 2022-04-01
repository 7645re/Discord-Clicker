using System.Collections.Generic;
using System.Threading.Tasks;
using discord_clicker.Models;
using Microsoft.EntityFrameworkCore;

namespace discord_clicker.Services
{
    public interface IItemHandler<T, VT> where T : class, IItem<T, VT>, new()
    {
        public Task<List<VT>> GetItemsList(DbSet<T> itemsContext);
        public Task<VT> CreateItem(Dictionary<string, object> parameters, DbSet<T> itemsContext);
        public Task<Dictionary<bool, string>> BuyItem(int userId, int itemId, decimal money, 
            DbSet<T> itemsContext);
    }
}

