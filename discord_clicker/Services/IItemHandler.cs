using System.Collections.Generic;
using System.Threading.Tasks;

namespace discord_clicker.Services
{
    public interface IItemHandler<T>
    {
        Task<List<T>> GetItemsList(int userId);
        // Task<List<T>> BuyItem(uint userId);
    }
}
