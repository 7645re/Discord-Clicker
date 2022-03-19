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
using discord_clicker.ViewModels;
using discord_clicker.Services;

namespace discord_clicker.Controllers
{
    /** Controller which calculate the economy in web application */
    [Route("api/[action]")]
    public class EconomyController : Controller
    {
        private UserHandler _userHandler;
        private ILogger _logger;
        private ItemHandler<Build, BuildModel, UserBuild> _buildHandler;
        private ItemHandler<Upgrade, UpgradeModel, UserUpgrade> _upgradeHandler;
        private ItemHandler<Achievement, AchievementModel, UserAchievement> _achievementHandler;
        private DatabaseContext _db;
        public EconomyController(DatabaseContext context, UserHandler userHandler, ItemHandler<Build, BuildModel, UserBuild> buildHandler, 
        ItemHandler<Upgrade, UpgradeModel, UserUpgrade> upgradeHandler, ItemHandler<Achievement, AchievementModel, UserAchievement> achievementHandler, ILogger<EconomyController> logger)
        {
            _logger = logger;
            _db = context;
            _userHandler = userHandler;
            _buildHandler = buildHandler;
            _achievementHandler = achievementHandler;
            _upgradeHandler = upgradeHandler;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Stats() {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            return Ok(await _userHandler.GetFullInfoById(userId));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Builds() {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            return Ok(await _buildHandler.GetItemsList(userId, _db.Builds));
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Upgrades() {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            return Ok(await _upgradeHandler.GetItemsList(userId, _db.Upgrades));
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Achievements() {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            return Ok(await _achievementHandler.GetItemsList(userId, _db.Achievements));
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> BuyBuild(int buildId, decimal money) {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            return Ok(await _buildHandler.BuyItem(userId, buildId, money, _db.Builds));
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> BuyUpgrade(int upgradeId, decimal money) {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            return Ok(await _upgradeHandler.BuyItem(userId, upgradeId, money, _db.Upgrades));
        }
    }
}