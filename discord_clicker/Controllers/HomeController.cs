using System;
using System.Linq;
using discord_clicker.Models;
using System.Threading.Tasks;
using discord_clicker.Models.Person;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using discord_clicker.ViewModels;

namespace discord_clicker.Controllers;

public class HomeController : Controller
{
    private readonly DatabaseContext _db;
    private ILogger _logger;
    private readonly IMemoryCache _cache;
    public HomeController(DatabaseContext context, ILogger<HomeController> logger, IMemoryCache memoryCache)
    {
        _db = context;
        _logger = logger;
        _cache = memoryCache;
    }
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }
}