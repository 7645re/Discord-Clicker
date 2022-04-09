using System;
using discord_clicker.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using discord_clicker.ViewModels;
using Microsoft.Extensions.Logging;

namespace discord_clicker.Services;

public class UserHandlerCachingDecorator : IUserHandler
{
    private readonly IMemoryCache _cache;
    private readonly DatabaseContext _db;
    private readonly ILogger _logger;
    private readonly IUserHandler _userHandler;
    
    public UserHandlerCachingDecorator(IUserHandler userHandler, IMemoryCache cache, 
        DatabaseContext db, ILogger<UserHandlerCachingDecorator> logger)
    {
        _userHandler = userHandler;
        _logger = logger;
        _db = db;
        _cache = cache;
    }

    /// <summary> Function to check whether a user with such a nickname exists </summary>
    public async Task<bool> ExistCheck(string nickname)
        {
            return await _userHandler.ExistCheck(nickname: nickname);
        }
    #nullable enable
    public async Task<UserModel?> GetUser(int? userId = null, string? name = null, string? password = null)
    {
        User? user = null;
        if (userId != null && !_cache.TryGetValue(userId, out _) || name != null && password != null && !_cache.TryGetValue(name, out _))
        {
            _logger.LogInformation("User were not found in the cache and were taken from the database");
            user = await _db.Users.Where(u => userId != null && name == null && password == null ? 
                    u.Id == userId : u.Nickname == name && u.Password == password)
                .Include(u => u.UserBuilds).ThenInclude(up => up.Item)
                .Include(u => u.UserUpgrades).ThenInclude(uu => uu.Item)
                .Include(u => u.UserAchievements).ThenInclude(ua => ua.Item)
                .Include(u => u.Role)
                .FirstOrDefaultAsync();
        }
        if (user != null)
        {
            _cache.Set(userId != null && name == null && password == null ? userId : name, user,
                new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
        }

        if (_cache.TryGetValue(userId+".UserModel", out UserModel? userModel))
        {
            return userModel;
        }
        userModel = await _userHandler.GetUser(userId: userId, name: name, password: password);
        _cache.Set(userId+".UserModel", userModel,
            new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
        return userModel;
    }

    public async Task<User> Create(string nickname, string password, decimal money, decimal clickCoefficient,
        decimal passiveCoefficient, DateTime playStartDate)
    {
        User user = await _userHandler.Create(nickname: nickname, password: password, 
            money: money, clickCoefficient: clickCoefficient, passiveCoefficient: passiveCoefficient,
            playStartDate: playStartDate);
        _cache.Set(user.Id, user, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
        return user;
    }    
        
    
}