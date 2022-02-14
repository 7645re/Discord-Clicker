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



namespace discord_clicker.Controllers
{
    public class EconomyController : Controller
    {
        private UserContext db;
        private IMemoryCache cache;
        public static Dictionary<int, DateTime> LastRequest = new Dictionary<int, DateTime>();
        private IConfiguration Configuration;
        public EconomyController(UserContext context, IMemoryCache memoryCache, IConfiguration configuration)
        {
            Configuration = configuration;
            db = context;
            cache = memoryCache;
        }
        private bool VerifyMoney(int userId, ulong money, uint ClickCoefficient, ulong PassiveCoefficient) {
            ulong userInterval = Convert.ToUInt64((DateTime.Now - LastRequest[userId]).TotalMilliseconds);
            ulong maxMoney = (ClickCoefficient*Convert.ToUInt16(Configuration["GeneralValues:MaxClickPerSecond"])+PassiveCoefficient)*userInterval;
            LastRequest[userId] = DateTime.Now;
            return maxMoney >= money*1000 ? true: false;
        }
        private void TrySetFirstRequestTime(int userId) {
            if (!EconomyController.LastRequest.ContainsKey(userId)) {
                EconomyController.LastRequest.Add(userId, DateTime.Now);
            }
        }
        // [Route("SetMoney")]
        // [Authorize]
        // public async Task<IActionResult> SetMoney(ulong money) {
        //     if (money > 0) {

        //         int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
        //         ulong userInterval = Convert.ToUInt64((DateTime.Now - LastRequest[userId]).TotalMilliseconds);
        //         User user = await db.Users.Where(u => u.Id == userId).FirstAsync();
        //         ulong dbMoney = user.Money;
        //         if (money*1000 <= userInterval*14*user.ClickCoefficient+userInterval*user.PassiveCoefficient)
        //         {
        //             user.Money += money;
        //             await db.SaveChangesAsync();
        //             LastRequest[userId] = DateTime.Now;
        //             return Json(new { result = "ok", money = dbMoney + money, r = userInterval, a = DateTime.Now, b = LastRequest[userId],
        //                 test=money*1000, testt=userInterval*14*user.ClickCoefficient
        //             });
        //         }
        //         else {
        //             LastRequest[userId] = DateTime.Now;
        //             return Json(new { result = "error", reason = "unnormal interval", money = dbMoney, r = userInterval });
        //         }
        //     }
        //     else
        //     {
        //         return Json(new { result = "error", reason="0 clicks" });
        //     }
        // }
        // [Route("GetMoney")]
        // [Authorize]
        // public async Task<IActionResult> GetMoney() {
        //     int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
        //     User user = await db.Users.Where(u => u.Id == userId).FirstAsync();
        //     return Json(new { money = user.Money });
        // }
        [Route("getUserInformation")]
        [Authorize]
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

                bool presenceRowInTable = user.UserPerks.Where(up => up.UserId == userId).FirstOrDefault() != null;
                if (!presenceRowInTable) {
                    user.UserPerks.Add(new UserPerk { UserId=userId, PerkId=perk.Id, Count=1});
                }
                buyedPerkCount = presenceRowInTable == true ? user.UserPerks.Where(up => up.UserId == userId).First().Count+=1: 1;
                user.PassiveCoefficient+=perk.PassiveCoefficient;
                user.ClickCoefficient+=perk.ClickCoefficient;
                user.Tier=perk.Tier+1;
                result = "ok";
                await db.SaveChangesAsync();
                cache.Set(userId.ToString()+".user", user, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
                cache.Set(userId.ToString()+".perksList", perk, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }
            if (availabilityСache)
            return Json(new {result, buyedPerkCount});
        }
        [Route("getPerksList")]
        [Authorize]
        async public Task<IActionResult> getPerksList() {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            List<Perk> perksList;
            Dictionary<int, uint> perksCount = new Dictionary<int, uint>();

            if (!(cache.TryGetValue(userId.ToString()+".perksList", out perksList) && cache.TryGetValue(userId.ToString()+".perksCount", out perksCount))) {
                perksList = new List<Perk>();
                List<Perk> perksListLinks = await db.Perks.Where(p => p.Cost > 0).Include(p => p.UserPerks).ToListAsync();
                foreach (Perk perk in perksListLinks)
                {
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
                cache.Set(userId.ToString()+".perksList", perksList, 
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(Convert.ToInt16(Configuration["GeneralValues:PerksCachingTime"]))));
                cache.Set(userId.ToString()+".perksCount", perksCount, 
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(Convert.ToInt16(Configuration["GeneralValues:PerksCachingTime"]))));
            }
            return Json(new { result = "ok", perksList, perksCount});
        }
    }
}
