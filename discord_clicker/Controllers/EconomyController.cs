using System;
using discord_clicker.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using discord_clicker.Models.Items.AchievementClasses;
using discord_clicker.Models.Items.BuildClasses;
using discord_clicker.Models.Items.UpgradeClasses;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using discord_clicker.ViewModels;
using discord_clicker.Services.ItemHandler;
using discord_clicker.Services.UserHandler;
using discord_clicker.Models.Person;

namespace discord_clicker.Controllers;

/** Controller which calculate the economy in web application */
[Route("/[action]")]
public class EconomyController : Controller
{
    private readonly IUserHandler _userHandler;
    private readonly ILogger _logger;
    private readonly IItemHandler<Build, BuildViewModel, BuildCreateModel> _buildHandler;
    private readonly IItemHandler<Upgrade, UpgradeViewModel, UpgradeCreateModel> _upgradeHandler;
    private readonly IItemHandler<Achievement, AchievementViewModel, AchievementCreateModel> _achievementHandler;
    private readonly DatabaseContext _db;
    public EconomyController(DatabaseContext context, IUserHandler userHandler, 
        IItemHandler<Build, BuildViewModel, BuildCreateModel> buildHandler, 
        IItemHandler<Upgrade, UpgradeViewModel, UpgradeCreateModel> upgradeHandler, 
        IItemHandler<Achievement, AchievementViewModel, AchievementCreateModel> achievementHandler, 
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
    public async Task<IActionResult> Stats()
    {
        int userId = Convert.ToInt32(HttpContext.User.Identity?.Name);
        return Ok(await _userHandler.GetUserView(userId));
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Builds()
    {
        return Ok(await _buildHandler.GetItemsList(_db.Builds));
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Upgrades()
    {
        return Ok(await _upgradeHandler.GetItemsList(_db.Upgrades));
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Achievements()
    {
        return Ok(await _achievementHandler.GetItemsList(_db.Achievements));
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> BuyBuild(int id, decimal money)
    {
        int userId = Convert.ToInt32(HttpContext.User.Identity?.Name);
        return Ok(await _buildHandler.BuyItem(userId, id, money, _db.Builds));
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> BuyUpgrade(int id, decimal money)
    {
        int userId = Convert.ToInt32(HttpContext.User.Identity?.Name);
        return Ok(await _upgradeHandler.BuyItem(userId, id, money, _db.Upgrades));
    }
    
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateBuild([FromBody] BuildCreateModel buildCreateModel)
    {
        BuildViewModel buildViewModel =  await _buildHandler.CreateItem(buildCreateModel, _db.Builds);
        return Ok(buildViewModel);
    }
    
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateUpgrade([FromBody] UpgradeCreateModel upgradeCreateModel)
    {
        UpgradeViewModel upgradeViewModel =  await _upgradeHandler.CreateItem(upgradeCreateModel, _db.Upgrades);
        return Ok(upgradeViewModel);
    }
    
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateAchievement([FromBody] AchievementCreateModel achievementCreateModel)
    {
        AchievementViewModel achievementViewModel =  await _achievementHandler.CreateItem(achievementCreateModel, _db.Achievements);
        return Ok(achievementViewModel);
    }
}