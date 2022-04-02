using System;
using System.Linq;
using discord_clicker.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using discord_clicker.ViewModels;

namespace discord_clicker.Controllers;

public class HomeController : Controller
{
    private readonly DatabaseContext _db;
    private ILogger _logger;
    private readonly IMemoryCache _cache;
    public HomeController(DatabaseContext context, ILogger<EconomyController> logger, IMemoryCache memoryCache)
    {
        _db = context;
        _logger = logger;
        _cache = memoryCache;
    }
    [Authorize]
    public async Task<IActionResult> Index()
    {
        int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
        #nullable enable
        User? user;
        bool availabilityСache = _cache.TryGetValue(userId.ToString(), out user);
        if (!availabilityСache) {
            user = await _db.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
        }
        if (user != null) {
            user.LastRequestDate = DateTime.Now;
            _cache.Set(userId.ToString(), user, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
        }
        else {
            return RedirectToAction("Register", "Account");
        }
        return View();
    }
}