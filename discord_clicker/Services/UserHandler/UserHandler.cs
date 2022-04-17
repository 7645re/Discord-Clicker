using System;
using System.Threading.Tasks;
using discord_clicker.Models;
using discord_clicker.Models.Person;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using AutoMapper;
using discord_clicker.ViewModels;


namespace discord_clicker.Services.UserHandler;

public class UserHandler : IUserHandler
{
    private readonly DatabaseContext _db;
    private readonly IMemoryCache _cache;
    private readonly IMapper _mapper;
    
    public UserHandler(DatabaseContext context, IMemoryCache memoryCache, IMapper mapper)
    {
        _db = context;
        _cache = memoryCache;
        _mapper = mapper;
    }

    public async Task<bool> ExistByName(string nickname)
    {
        #nullable enable
        return (await _db.Users.FirstOrDefaultAsync(u => u.Nickname == nickname) != null);
    }

    #nullable enable
    public async Task<User?> GetUserAuthAsync(string name, string password)
    {
        return await _db.Users.Where(u => u.Nickname == name && u.Password == password)
            .Include(u => u.Role).FirstOrDefaultAsync();
    }
    
    #nullable enable
    public async Task<User?> GetUserAsync(int id)
    {
        return await _db.Users.Where(u => u.Id == id)
            .Include(u => u.UserBuilds).ThenInclude(up => up.Item)
            .Include(u => u.UserUpgrades).ThenInclude(uu => uu.Item)
            .Include(u => u.UserAchievements).ThenInclude(ua => ua.Item).Include(u => u.Role)
            .FirstOrDefaultAsync();
    }

    public async Task<UserViewModel?> GetUserView(int id)
    {
        User? user = await GetUserAsync(id);
        return _mapper.Map<UserViewModel>(user);
    }

    public async Task<User> CreateUserAsync(RegisterModel registerModel)
    {
        User user = _mapper.Map<User>(registerModel);
        user.PlayStartDate = DateTime.UtcNow;
        Role userRole = await _db.Roles.FirstAsync(r => r.Name == "user");
        user.Role = userRole;
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
        return user;
    }
}