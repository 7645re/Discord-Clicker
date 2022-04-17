using System.Threading.Tasks;
using discord_clicker.Models.Person;
using discord_clicker.ViewModels;

namespace discord_clicker.Services.UserHandler;

public interface IUserHandler
{
    /// <summary>
    /// check for the existence of a user with such a nickname
    /// </summary>
    public Task<bool> ExistByName(string nickname);
    #nullable enable
    /// <summary>
    /// getting a user to manage item purchases, with dependencies
    /// </summary>
    public Task<User?> GetUserAsync(int userId);
    /// <summary>
    /// getting a user for auth, without dependencies
    /// </summary>
    public Task<User?> GetUserAuthAsync(string name, string password);

    public Task<User> CreateUserAsync(RegisterModel registerModel);
    public Task<UserViewModel?> GetUserView(int id);
}
