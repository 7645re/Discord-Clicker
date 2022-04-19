using System.Threading.Tasks;
using discord_clicker.Models.Person;
using discord_clicker.ViewModels;

namespace discord_clicker.Services.UserHandler;

public interface IUserHandler
{
    public Task<bool> ExistByName(string nickname);
    #nullable enable
    public Task<User?> GetUserAsync(int userId);
    public Task<User?> GetUserAuthAsync(string name, string password);
    public Task<User> CreateUserAsync(RegisterModel registerModel);
    public Task<UserViewModel?> GetUserView(int id);
}
