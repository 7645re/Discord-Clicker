using System;
using System.Linq;
using discord_clicker.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace discord_clicker.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserContext db;
        private ILogger _logger;
        private IMemoryCache cache;
        public HomeController(UserContext context, ILogger<EconomyController> logger, IMemoryCache memoryCache)
        {
            db = context;
            _logger = logger;
            cache = memoryCache;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            User user;
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            bool availabilityСache = cache.TryGetValue(userId.ToString()+".user", out user);

            if (!availabilityСache) {
                /** if the data could not be found in the cache, we turn to the database */
                user = db.Users.Where(u => u.Id == userId).First();
            }
            user.LastRequestDate = DateTime.Now;
            if (!availabilityСache) {
                cache.Set(userId.ToString()+".user", user, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }
            return View();
        }
    }
}