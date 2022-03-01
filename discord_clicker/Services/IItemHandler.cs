using System.Collections.Generic;
using System.Threading.Tasks;

namespace discord_clicker.Services
{
    public interface IItemHandler<T>
    {
        Task<List<T>> GetItemsList(int userId);
        Task<Dictionary<string, object>> BuyItem(int userId, int itemId, decimal money);
    }
}
