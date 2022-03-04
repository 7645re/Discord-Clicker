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

namespace discord_clicker.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserContext _db;
        private ILogger _logger;
        private IMemoryCache _cache;
        public HomeController(UserContext context, ILogger<EconomyController> logger, IMemoryCache memoryCache)
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
                /** if the data could not be found in the cache, we turn to the database */
                user = await _db.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            }
            if (user != null) {
                user.LastRequestDate = DateTime.Now;
                _cache.Set(userId.ToString(), user, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }
            else {
                /** If the user was removed from the database but he still has data in the browser cache, then he redirects to the registration page */
                return RedirectToAction("Register", "Account");
            }
            return View();
        }
    }
}