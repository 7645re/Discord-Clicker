using System.Threading.Tasks;

namespace discord_clicker.Services
{
    public interface IPersonHandler<T>
    {
        Task<T> GetInformation(int Id);
    }
}
