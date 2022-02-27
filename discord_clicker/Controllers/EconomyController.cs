using System;
using System.Linq;
using discord_clicker.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using discord_clicker.Services;
using discord_clicker.ViewModels;

namespace discord_clicker.Controllers
{
    /** Controller which calculate the economy in web application */
    [Route("api/[action]")]
    public class EconomyController : Controller
    {
        private IItemHandler<BuildModel> _buildHandler;
        public EconomyController(IItemHandler<BuildModel> buildHandler)
        {
            _buildHandler = buildHandler;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserInformation() {
            User user;
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            if (!cache.TryGetValue(userId.ToString()+".user", out user)) {
                user = await db.Users.Where(u => u.Id == userId).FirstAsync();
                cache.Set(userId.ToString() + ".user", user);
            }
            user.Password = ""; /** Removing a password from a class instance */
            return Json(new { status = "ok", user= user });
        }


        [Authorize]
        [HttpGet]
        async public Task<IActionResult> BuyBuild(int buildId, ulong money)
        {       
            User user; /** Get either from the cache or from the database */
            Build build = null; /** Get either from the buildsList or from the database */
            // bool verifyMoney; /** Could a user earn that much money in a period of time */
            // bool rankSatisfy; /** Does the user's rank satisfy the purchase of this ability */
            bool enoughMoney; /** Does the user have enough money to buy the ability */
            bool availabilityСache; /** Availability of data in the cache */
            bool presenceRowInTable; /** Has the user bought this ability before. Does it exist in the database */
            uint buyedBuildCount = 0; /** The number of purchased this ability */
            Dictionary<int, uint> buildsCount; /** The list of ability IDs and the number of purchased ones that exist is taken only from the cache. */
            List<Build> buildsList = new List<Build>(); /** The list of abilities that exist is taken either from the cache or from the database */
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name); /** User ID */
            
            availabilityСache = cache.TryGetValue(userId.ToString()+".user", out user) && cache.TryGetValue(userId.ToString() + ".buildsList", out buildsList);

            if (!availabilityСache) {
                /** if the data could not be found in the cache, we turn to the database */
                build = await db.Builds.Where(p => p.Id == buildId).FirstOrDefaultAsync();
                user = await db.Users.Include(u => u.UserBuilds).Where(u => u.Id == userId).FirstAsync();
                _logger.LogInformation("User and buildList were not found in the cache and were taken from the database");
            }
            else {
                build = buildsList.Where(p => p.Id == buildId).FirstOrDefault();
                _logger.LogInformation("User and buildsList were taken from cache");
            }
            if (money == 0) {
                _logger.LogInformation($"A user with ID {user.Id} sent a request to purchase an item with ID {build.Id} with 0 money");
                return Json(new {status="error", reason="When sending requests, the user has 0 coins"});
            }
            if (build == null) {
                _logger.LogInformation($"A user with ID {user.Id} has sent a request to purchase an item with ID {build.Id} that does not exist");
                return Json(new {status="error", reason=$"The ability with ID {buildId} does not exist"});
            }

            // verifyMoney = VerifyMoney(user.LastRequestDate, user.Money, money, user.ClickCoefficient,  user.PassiveCoefficient);
            presenceRowInTable = user.UserBuilds.Where(up => up.UserId == userId && up.BuildId == buildId).FirstOrDefault() != null; 
            buyedBuildCount = !presenceRowInTable ? 1 : user.UserBuilds.Where(up => up.UserId == userId && up.BuildId == buildId).First().Count+1;
            enoughMoney = money >= build.Cost*buyedBuildCount;

            if (!enoughMoney) {
                return Json(new {status="error", reason="There are not enough funds to buy the ability"});
            }
            // if (!verifyMoney) {
            //     return Json(new {result="cheat", reason=$"You could not earn {money-user.Money} coins in a time interval of {(DateTime.Now-user.LastRequestDate).TotalMilliseconds}s", money=Convert.ToDecimal((DateTime.Now-user.LastRequestDate).TotalMilliseconds/1000*user.PassiveCoefficient),
            //     user.ClickCoefficient, user.PassiveCoefficient});
            // }
            if (!presenceRowInTable) {
                user.UserBuilds.Add(new UserBuild { UserId=userId, BuildId=build.Id, Count=1});
            }
            else {
                user.UserBuilds.Where(up => up.UserId == userId && up.BuildId == buildId).First().Count+=1;
            }

            user.Builds.Add(build);
            user.LastRequestDate = DateTime.Now;
            user.PassiveCoefficient+=build.PassiveCoefficient;
            user.Money=money-build.Cost*(buyedBuildCount == 0 ? 1 : buyedBuildCount);

            if (cache.TryGetValue(userId.ToString()+".buildsCount", out buildsCount)) {
                buildsCount[buildId]+=1;
                cache.Set(userId.ToString()+".buildsCount", buildsCount, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }
            if (!availabilityСache) {
                cache.Set(userId.ToString()+".user", user, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }
                _logger.LogInformation($"The user with ID {user.Id} bought an ability with ID {build.Id} for {buildsCount[buildId]*build.Cost} money and now has {buildsCount[buildId]} pieces");
            return Json(new {status="ok", reason=$"The user with ID {user.Id} bought an ability with ID {build.Id} for {buildsCount[buildId]*build.Cost} money and now has {buildsCount[buildId]} pieces", user.ClickCoefficient, user.PassiveCoefficient, buyedBuildCount, build.Cost, user.Money, build.Name});
        }

        [Authorize]
        [HttpGet]
        async public Task<IActionResult> getBuildsList() {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name); /** User ID */
            return Ok(await _buildHandler.GetItemsList(userId));
        }
    }
}