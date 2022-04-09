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
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using discord_clicker.ViewModels;
using discord_clicker.Services;

namespace discord_clicker.Controllers;

/** Controller which calculate the economy in web application */
[Route("/[action]")]
public class EconomyController : Controller
{
    private readonly IUserHandler _userHandler;
    private readonly ILogger _logger;
    private readonly IItemHandler<Build, BuildModel> _buildHandler;
    private readonly IItemHandler<Upgrade, UpgradeModel> _upgradeHandler;
    private readonly IItemHandler<Achievement, AchievementModel> _achievementHandler;
    private readonly DatabaseContext _db;
    public EconomyController(DatabaseContext context, IUserHandler userHandler, 
        IItemHandler<Build, BuildModel> buildHandler, 
        IItemHandler<Upgrade, UpgradeModel> upgradeHandler, 
        IItemHandler<Achievement, AchievementModel> achievementHandler, 
        ILogger<EconomyController> logger)
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
        return Ok(await _userHandler.GetUser(userId));
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Builds() {
        return Ok(await _buildHandler.GetItemsList(_db.Builds));
    }
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Upgrades() {
        return Ok(await _upgradeHandler.GetItemsList(_db.Upgrades));
    }
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Achievements() {
        return Ok(await _achievementHandler.GetItemsList(_db.Achievements));
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
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateBuild(int id, string name, decimal cost, 
        string description, decimal passiveCoefficient) {
        Dictionary<string, object> parameters = new Dictionary<string, object>() {
            {"Id", id},
            {"Name", name},
            {"Cost", cost},
            {"Description", description},
            {"PassiveCoefficient", passiveCoefficient}
        };
        BuildModel item = await _buildHandler.CreateItem(parameters, _db.Builds);
        return Ok(item);
    }
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateUpgrade(int id, string name, decimal cost, uint buildId, string action,
        uint conditionGet, bool forEachBuild, string description, decimal getMoney) {
        Dictionary<string, object> parameters = new Dictionary<string, object>() {
            {"Id", id},
            {"Name", name},
            {"Cost", cost},
            {"BuildId", buildId},
            {"Action", action},
            {"ConditionGet", conditionGet},
            {"ForEachBuild", forEachBuild},
            {"GetMoney", getMoney},
            {"Description", description},
        };
        UpgradeModel item = await _upgradeHandler.CreateItem(parameters, _db.Upgrades);
        return Ok(item);
    }
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateAchievement(int id, string name, string description, 
        string achievementObjectType, uint achievementObjectId, decimal achievementObjectCount) {
        Dictionary<string, object> parameters = new Dictionary<string, object>() {
            {"Id", id},
            {"Name", name},
            {"Description", description},
            {"AchievementObjectType", achievementObjectType},
            {"AchievementObjectId", achievementObjectId},
            {"AchievementObjectCount", achievementObjectCount}
        };
        AchievementModel item = await _achievementHandler.CreateItem(parameters, _db.Achievements);
        return Ok(item);
    }
}