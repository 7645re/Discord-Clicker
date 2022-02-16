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
        public IActionResult Index()
        {
            return View();
        }
    }
}