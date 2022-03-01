using System.Threading.Tasks;
using discord_clicker.ViewModels;

namespace discord_clicker.Services
{
    public interface IPersonHandler<T>
    {
        Task<T> GetInfoById(int id);
        Task<T> GetInfoByName(string nickname);
        Task<T> GetInfoByPass(string nickname, string passowrd);
        Task<T> Create(string nickname, string password, decimal money, decimal clickCoefficient, 
            decimal passiveCoefficient);
    }
}
