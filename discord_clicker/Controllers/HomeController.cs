using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using discord_clicker.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace discord_clicker.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserContext db;
        public HomeController(UserContext context)
        {
            db = context;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            if (!EconomyController.LastRequest.ContainsKey(userId)) {
                EconomyController.LastRequest.Add(userId, DateTime.Now);
            }

            List<Perk> perks = await db.Perks.Include(up => up.UserPerks).Where(p => p.Cost > 0).ToListAsync();
            ViewBag.Perks = perks;
            return View();
        }
    }
}