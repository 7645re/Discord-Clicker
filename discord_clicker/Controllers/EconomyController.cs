using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using discord_clicker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using discord_clicker.Serializer;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace discord_clicker.Controllers
{
    public class EconomyController : Controller
    {
        private UserContext db;
        public static Dictionary<int, DateTime> LastRequest = new Dictionary<int, DateTime>();
        public EconomyController(UserContext context)
        {
            db = context;
        }
        [Route("SetMoney")]
        [Authorize]
        public async Task<IActionResult> SetMoney(ulong money) {
            if (money > 0) {
                int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
                ulong userInterval = Convert.ToUInt64((DateTime.Now - LastRequest[userId]).TotalMilliseconds);
                User user = await db.Users.Where(u => u.Id == userId).FirstAsync();
                ulong dbMoney = user.Money;
                if (money*1000 <= userInterval*14*user.ClickCoefficient+userInterval*user.PassiveCoefficient)
                {
                    user.Money += money;
                    await db.SaveChangesAsync();
                    LastRequest[userId] = DateTime.Now;
                    return Json(new { result = "ok", money = dbMoney + money, r = userInterval, a = DateTime.Now, b = LastRequest[userId],
                        test=money*1000, testt=userInterval*14*user.ClickCoefficient
                    });
                }
                else {
                    LastRequest[userId] = DateTime.Now;
                    return Json(new { result = "error", reason = "unnormal interval", money = dbMoney, r = userInterval });
                }
                    
            }
            else
            {
                return Json(new { result = "error", reason="No clicks detected" });
            }
        }
        [Route("GetMoney")]
        [Authorize]
        public async Task<IActionResult> GetMoney() {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            User user = await db.Users.Where(u => u.Id == userId).FirstAsync();
            return Json(new { money = user.Money });
        }
        [Route("getUserInformation")]
        [Authorize]
        public async Task<IActionResult> GetUserInformation() {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            User user = await db.Users.Where(u => u.Id == userId).FirstAsync();
            return Json(new { user = user });
        }

        [Route("buyPerk")]
        [Authorize]
        async public Task<IActionResult> BuyPerk(int perkId)
        {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            User user = await db.Users.Include(u => u.UserPerks).Where(u => u.Id == userId).FirstAsync();
            Perk perk = await db.Perks.Where(p => p.Id == perkId).FirstAsync();
            Perk buyedPerk = user.Perks.FirstOrDefault(bp => bp.Id == perkId);
            uint buyedPerkCount = 0;
            if (user.Tier >= perk.Tier && user.Money >= perk.Cost*(buyedPerk == null ? 1: user.UserPerks.Where(p => p.PerkId == perkId).First().Count))
            {
                if (buyedPerk == null)
                {
                    user.Perks.Add(perk);
                    user.Tier = perk.Tier+1;
                    await db.SaveChangesAsync();
                    user.UserPerks.Where(p => p.PerkId == perkId).First().Count += 1;
                }
                else {
                    user.UserPerks.Where(p => p.PerkId == perkId).First().Count += 1;
                }
                buyedPerkCount = user.UserPerks.First((p) => p.PerkId == perkId).Count;
                user.ClickCoefficient += perk.ClickCoefficient;
                user.PassiveCoefficient += perk.PassiveCoefficient;
                user.Money-=perk.Cost * (buyedPerkCount == 1 ? 1 : buyedPerkCount-1 );
                await db.SaveChangesAsync();
                return Json(new { result="ok", userMoney=user.Money, clickCoefficient=user.ClickCoefficient, buyedPerkCount = buyedPerkCount, perkCost=perk.Cost,
                perkName=perk.Name, passiveCoefficient=user.PassiveCoefficient });
            }
            else {
                return Json(new { result="error" });
            }
        }
        [Route("getPerksList")]
        [Authorize]
        async public Task<IActionResult> getPerksList() {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            List<Perk> perksListLinks = await db.Perks.Where(p => p.Cost > 0).Include(p => p.UserPerks).ToListAsync();
            List<Perk> perksList = new List<Perk>();
            Dictionary<int, uint> perksCount = new Dictionary<int, uint>();
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
                perksCount.Add(perk.Id, 
                perk.UserPerks.Where(p => p.UserId == userId).ToList().Count > 0 ? perk.UserPerks.First().Count: 0);

            }
            return Json(new { result = "ok", perksList = perksList, perksCount = perksCount });
        }
    }
}
