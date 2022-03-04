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
        private IItemHandler<UpgradeModel> _upgradeHandler;
        private IPersonHandler<User, UserModel> _userHandler;
        public EconomyController(IItemHandler<BuildModel> buildHandler, IPersonHandler<User, UserModel> userHandler, IItemHandler<UpgradeModel> upgradeHandler)
        {
            _buildHandler = buildHandler;
            _userHandler = userHandler;
            _upgradeHandler = upgradeHandler;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Stats() {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            return Ok(await _userHandler.GetFullInfoById(userId));
        }


        [Authorize]
        [HttpGet]
        async public Task<IActionResult> BuyBuild(int buildId, ulong money) {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name); /** User ID */
            return Ok(await _buildHandler.GetItem(userId: userId, itemId: buildId, money: money));
        }

        [Authorize]
        [HttpGet]
        async public Task<IActionResult> BuyUpgrade(int upgradeId, ulong money) {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name); /** User ID */
            return Ok(await _upgradeHandler.GetItem(userId: userId, itemId: upgradeId, money: money));
        }

        [Authorize]
        [HttpGet]
        async public Task<IActionResult> Builds() {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name); /** User ID */
            return Ok(await _buildHandler.GetItemsList(userId));
        }
        [Authorize]
        [HttpGet]
        async public Task<IActionResult> Upgrades() {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name); /** User ID */
            return Ok(await _upgradeHandler.GetItemsList(userId));
        }
    }
}