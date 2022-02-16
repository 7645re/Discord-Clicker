using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using discord_clicker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using discord_clicker.Serializer;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace discord_clicker.Controllers
{
    /** Controller which calculate the economy in web application */
    public class EconomyController : Controller
    {
        private UserContext db;
        private IMemoryCache cache;
        /** Dictionary for contain time of users requests to server */
        public static Dictionary<int, DateTime> LastRequest = new Dictionary<int, DateTime>();
        private readonly IConfiguration Configuration;
        private readonly ILogger _logger;
        public EconomyController(UserContext context, IMemoryCache memoryCache, IConfiguration configuration, ILogger<EconomyController> logger)
        {
            Configuration = configuration;
            db = context;
            cache = memoryCache;
            _logger = logger;
        }
        /** Function verify money response from client, 
         * checking on autoclick or unnormal money growth */
        private bool VerifyMoney(int userId, ulong money, uint ClickCoefficient, ulong PassiveCoefficient) {
            ulong userInterval = Convert.ToUInt64((DateTime.Now - LastRequest[userId]).TotalMilliseconds);
            ulong maxMoney = (ClickCoefficient*Convert.ToUInt16(Configuration["GeneralValues:MaxClickPerSecond"])+PassiveCoefficient)*userInterval;
            LastRequest[userId] = DateTime.Now;
            return maxMoney >= money*1000 ? true: false;
        }
        /** Function is try add time of last request and after calculate 
         * how much client can earn money in this interval */
        private void TrySetFirstRequestTime(int userId) {
            if (!EconomyController.LastRequest.ContainsKey(userId)) {
                EconomyController.LastRequest.Add(userId, DateTime.Now);
            }
        }
        [Route("getUserInformation")]
        [Authorize]
        /** Function return information about user (User object) from cache or database */
        public async Task<IActionResult> GetUserInformation() {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            User user;
            if (!cache.TryGetValue(userId.ToString()+".user", out user)) {
                user = await db.Users.Where(u => u.Id == userId).FirstAsync();
            }
            return Json(new { user = user });
        }

        [Authorize]
        [Route("buyPerk")]
        async public Task<IActionResult> BuyPerk(int perkId, ulong money)
        {   
            string result = "error";
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            User user;
            Perk perk = null;
            uint buyedPerkCount = 0;
            /** Availability of data in the cache */
            bool availabilityСache = cache.TryGetValue(userId.ToString()+".user", out user) && cache.TryGetValue(userId.ToString()+".perksList", out perk);
            if (!availabilityСache) {
                user = await db.Users.Include(u => u.UserPerks).Where(u => u.Id == userId).FirstAsync();
                perk = await db.Perks.Where(p => p.Id == perkId).FirstOrDefaultAsync();
            }

            TrySetFirstRequestTime(userId);

            if (VerifyMoney(userId, money, user.ClickCoefficient,  user.PassiveCoefficient) && 
                perk != null && user.Tier >= perk.Tier && user.Money+money>=perk.Cost) {
                user.Money=user.Money+money-perk.Cost;
                user.Perks.Add(perk);

                /** Сhecking the presence of a row in the table */
                bool presenceRowInTable = user.UserPerks.Where(up => up.UserId == userId).FirstOrDefault() != null;
                if (!presenceRowInTable) {
                    user.UserPerks.Add(new UserPerk { UserId=userId, PerkId=perk.Id, Count=1});
                }
                buyedPerkCount = presenceRowInTable == true ? user.UserPerks.Where(up => up.UserId == userId).First().Count+=1: 1;

                user.PassiveCoefficient+=perk.PassiveCoefficient;
                user.ClickCoefficient+=perk.ClickCoefficient;
                user.Tier=perk.Tier+1;

                result = "ok";
                
                /** Set data in the cache without a lifetime limit */
                cache.Set(userId.ToString()+".user", user, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
                cache.Set(userId.ToString()+".perksList", perk, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));

                _logger.LogInformation((((User)cache.Get(userId.ToString()+".user")).Money).ToString());        
            }
            return Json(new {result, buyedPerkCount, user.Money});
        }
        [Route("getPerksList")]
        [Authorize]
        /** This is a function that gets data about perks from the cache or database */
        async public Task<IActionResult> getPerksList() {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            List<Perk> perksList;
            Dictionary<int, uint> perksCount = new Dictionary<int, uint>();
            /** Сhecking for data in the cache */
            if (!(cache.TryGetValue(userId.ToString()+".perksList", out perksList) && cache.TryGetValue(userId.ToString()+".perksCount", out perksCount))) {
                perksList = new List<Perk>();
                List<Perk> perksListLinks = await db.Perks.Where(p => p.Cost > 0).Include(p => p.UserPerks).ToListAsync();
                foreach (Perk perk in perksListLinks)
                {
                    /** Function create new instance cuz i select data with relationships many-to-many.
                     *  Serializer work recursive and go to infinity loop cuz tables have link to each other.
                     * if i need again recreate instance i will use automapper */
                    perksList.Add(new Perk()
                    {
                        Id = perk.Id,
                        Cost = perk.Cost,
                        Name = perk.Name,
                        PassiveCoefficient = perk.PassiveCoefficient,
                        ClickCoefficient = perk.ClickCoefficient,
                        Tier = perk.Tier
                    });
                    perksCount.Add(perk.Id, perk.UserPerks.Where(p => p.UserId == userId).ToList().Count > 0 ? perk.UserPerks.First().Count: 0);
                }
                /**set option to never remove user from cache*/
                cache.Set(userId.ToString()+".perksList", perksList, 
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(Convert.ToInt16(Configuration["GeneralValues:PerksListCachingTime"]))));
                cache.Set(userId.ToString()+".perksCount", perksCount, 
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(Convert.ToInt16(Configuration["GeneralValues:PerksListCachingTime"]))));
            }
            return Json(new { result = "ok", perksList, perksCount});
        }
    }
}
