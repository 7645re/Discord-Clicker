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
        private bool VerifyMoney(DateTime userLastRequest, ulong uMoney, ulong money, uint ClickCoefficient, ulong PassiveCoefficient) {
            double userInterval = Convert.ToDouble((DateTime.Now - userLastRequest).TotalMilliseconds)/1000;
            double maxMoney = Convert.ToDouble((ClickCoefficient*Convert.ToUInt16(Configuration["GeneralValues:MaxClickPerSecond"])+PassiveCoefficient)*userInterval);
            return maxMoney >= money-uMoney;
        }

        [Route("getUserInformation")]
        [Authorize]
        /** Function return information about user (User object) from cache or database */
        public async Task<IActionResult> GetUserInformation() {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            User user;
            if (!cache.TryGetValue(userId.ToString()+".user", out user)) {
                user = await db.Users.Where(u => u.Id == userId).FirstAsync();
                cache.Set(userId.ToString() + ".user", user);
            }
            return Json(new { user = user });
        }

        [Authorize]
        [Route("buyPerk")]
        /// <summary>
        ///  Fhe function purchases an ability (Perk). At the same time, it saves the 
        ///  data that has not been saved to the database, more specifically money
        /// </summary>
        /// <param name="perkId">Id of the ability that the user wants to buy</param>
        /// <param name="money">Money that hasn't been saved yet</param>
        /// <returns>
        ///  Json response to ajax
        ///  The function should return new data about user data ClickCoefficient, PassiveCoefficient, buyedPerkCount, PerkCost
        /// </returns>
        async public Task<IActionResult> BuyPerk(int perkId, ulong money)
        {   
            string result = "error";
            if (money > 0) {
                int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
                User user; /** Get either from the cache or from the database */
                Perk perk = null; /** Get either from the perksList or from the database */
                List<Perk> perksList = new List<Perk>();
                Dictionary<int, uint> perksCount = new Dictionary<int, uint>();
                uint buyedPerkCount = 0;
                bool presenceRowInTable;
                bool verifyMoney;
                /** Availability of data in the cache */
                bool availabilityСache = cache.TryGetValue(userId.ToString()+".user", out user) && cache.TryGetValue(userId.ToString() + ".perksList", out perksList);
                if (!availabilityСache) {
                    user = await db.Users.Include(u => u.UserPerks).Where(u => u.Id == userId).FirstAsync();
                    perk = await db.Perks.Where(p => p.Id == perkId).FirstOrDefaultAsync();
                }
                else {
                    perk = perksList.Where(p => p.Id == perkId).First();
                }
                /** Сhecking the presence of a row in the table */
                presenceRowInTable = user.UserPerks.Where(up => up.UserId == userId && up.PerkId == perkId).FirstOrDefault() != null;
                buyedPerkCount = !presenceRowInTable ? 0 : user.UserPerks.Where(up => up.UserId == userId && up.PerkId == perkId).First().Count;
                verifyMoney = VerifyMoney(user.LastRequestDate, user.Money, money, user.ClickCoefficient,  user.PassiveCoefficient);

                if (verifyMoney) {
                    if (perk != null && user.Tier >= perk.Tier && money>=perk.Cost*(buyedPerkCount == 0 ? 1 : buyedPerkCount)) {
                        /** Сhecking the presence of a row in the table */
                        if (!presenceRowInTable) {
                            user.UserPerks.Add(new UserPerk { UserId=userId, PerkId=perk.Id, Count=1});
                        }
                        else {
                            user.UserPerks.Where(up => up.UserId == userId && up.PerkId == perkId).First().Count+=1;
                        }
                        user.Money=money-perk.Cost*(buyedPerkCount == 0 ? 1 : buyedPerkCount);
                        user.Perks.Add(perk);
                        user.PassiveCoefficient+=perk.PassiveCoefficient;
                        user.ClickCoefficient+=perk.ClickCoefficient;
                        user.Tier=perk.Tier+1 < user.Tier ? user.Tier : perk.Tier+1;
                        result = "ok";
                        /** Set in database new time of user request */
                        user.LastRequestDate = DateTime.Now;

                        if (cache.TryGetValue(userId.ToString()+".perksCount", out perksCount)) {
                            perksCount[perkId]+=1;
                        }
                        if (!availabilityСache) {
                        cache.Set(userId.ToString()+".user", user, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
                        }
                    }
                    return Json(new {result, buyedPerkCount=buyedPerkCount+1, user.Money, ClickCoefficient=user.ClickCoefficient,
                        PassiveCoefficient=user.PassiveCoefficient, PerkCost=perk.Cost});
                }
                else {
                    return Json(new {result="cheat", buyedPerkCount=buyedPerkCount, user.Money, ClickCoefficient=user.ClickCoefficient,
                        PassiveCoefficient=user.PassiveCoefficient, PerkCost=perk.Cost});
                }

            }
            return Json(new {error="zero"});
        }
        [Route("getPerksList")]
        [Authorize]
        /// <summary>
        ///  This is a function that gets data about perks from the cache or database
        /// </summary>
        /// <returns>ClickCoefficient, PassiveCoefficient, Money,</returns>
        async public Task<IActionResult> getPerksList() {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            List<Perk> perksList;
            Dictionary<int, uint> perksCount = new Dictionary<int, uint>();

            /** Сhecking for data in the cache */
            bool availabilityСache = cache.TryGetValue(userId.ToString() + ".perksList", out perksList) && cache.TryGetValue(userId.ToString() + ".perksCount", out perksCount);
            if (!availabilityСache)
            {
                perksList = new List<Perk>();
                List<Perk> perksListLinks = await db.Perks.Where(p => p.ClickCoefficient >= 0).Include(p => p.UserPerks).ToListAsync();
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
                    perksCount.Add(perk.Id, perk.UserPerks.Where(p => p.UserId == userId).ToList().Count > 0 ? perk.UserPerks.First().Count : 0);
                }
                /** Set option to never remove user from cache */
                cache.Set(userId.ToString() + ".perksList", perksList,
                    new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
                cache.Set(userId.ToString() + ".perksCount", perksCount,
                new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }
            return Json(new { result = "ok", perksList, perksCount});
        }
    }
}
