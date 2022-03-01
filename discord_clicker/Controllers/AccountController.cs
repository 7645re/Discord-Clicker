using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using discord_clicker.ViewModels;
using discord_clicker.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using discord_clicker.Services;
using System;

namespace discord_clicker.Controllers
{
    public class AccountController : Controller
    {
        private UserContext _db;
        private IPersonHandler<UserModel> _userHandler;
        public AccountController(UserContext context, IPersonHandler<UserModel> userHandler)
        {
            _db = context;
            _userHandler = userHandler;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel userModel = await _userHandler.GetInfoByPass(model.Nickname, model.Password);
                if (userModel != null)
                {
                    await Authenticate(userModel);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel userModel = await _userHandler.GetInfoByName(model.Nickname);
                if (userModel == null)
                {
                    userModel = await _userHandler.Create(nickname: model.Nickname, password: model.Password, money: 0, clickCoefficient: 1, passiveCoefficient: 0 );
                    await Authenticate(userModel);
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            return View(model);
        }

        private async Task Authenticate(UserModel userModel)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, Convert.ToString(userModel.Id))
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}