using System;
using System.Threading.Tasks;
using discord_clicker.ViewModels;
using discord_clicker.Models;

namespace discord_clicker.Services;

public interface IUserHandler
{
    public Task<bool> ExistCheck(string nickname);
    #nullable enable
    public Task<UserModel?> GetUser(int? userId = null, string? name = null, string? password = null);
    public Task<User> Create(string nickname, string password, decimal money, decimal clickCoefficient,
        decimal passiveCoefficient, DateTime playStartDate);
}
