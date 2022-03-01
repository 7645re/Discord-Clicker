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
        private IPersonHandler<UserModel> _userHandler;
        public EconomyController(IItemHandler<BuildModel> buildHandler, IPersonHandler<UserModel> userHandler)
        {
            _buildHandler = buildHandler;
            _userHandler = userHandler;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserInformation() {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            return Ok(await _userHandler.GetInfoById(userId));
        }


        [Authorize]
        [HttpGet]
        async public Task<IActionResult> BuyBuild(int buildId, ulong money) {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name); /** User ID */
            return Ok(await _buildHandler.BuyItem(userId: userId, itemId: buildId, money: money));
        }

        [Authorize]
        [HttpGet]
        async public Task<IActionResult> getBuildsList() {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name); /** User ID */
            return Ok(await _buildHandler.GetItemsList(userId));
        }
    }
}