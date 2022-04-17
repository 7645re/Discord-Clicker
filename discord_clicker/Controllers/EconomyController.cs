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
    private readonly IItemHandler<Build, BuildViewModel> _buildHandler;
    private readonly IItemHandler<Upgrade, UpgradeViewModel> _upgradeHandler;
    private readonly IItemHandler<Achievement, AchievementViewModel> _achievementHandler;
    private readonly DatabaseContext _db;
    public EconomyController(DatabaseContext context, IUserHandler userHandler, 
        IItemHandler<Build, BuildViewModel> buildHandler, 
        IItemHandler<Upgrade, UpgradeViewModel> upgradeHandler, 
        IItemHandler<Achievement, AchievementViewModel> achievementHandler, 
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
    //
    // [HttpGet]
    // [Authorize]
    // public async Task<IActionResult> BuyBuild(int id, decimal money)
    // {
    //     int userId = Convert.ToInt32(HttpContext.User.Identity?.Name);
    //     return Ok(await _buildHandler.BuyItem(userId, id, money, _db.Builds));
    // }
    //
    // [HttpGet]
    // [Authorize]
    // public async Task<IActionResult> BuyUpgrade(int id, decimal money)
    // {
    //     int userId = Convert.ToInt32(HttpContext.User.Identity?.Name);
    //     return Ok(await _upgradeHandler.BuyItem(userId, id, money, _db.Upgrades));
    // }
    //
    // [HttpGet]
    // [Authorize(Roles = "admin")]
    // public async Task<IActionResult> CreateBuild([FromQuery] BuildModel userModel)
    // {
    //     return Ok(1);
    // }
    //
    // [HttpGet]
    // [Authorize(Roles = "admin")]
    // public async Task<IActionResult> CreateUpgrade([FromQuery] UpgradeModel upgradeModel)
    // {
    //     return Ok(1);
    // }
    //
    // [HttpGet]
    // [Authorize(Roles = "admin")]
    // public async Task<IActionResult> CreateAchievement([FromQuery] AchievementModel achievementModel)
    // {
    //     return Ok(1);
    // }
}