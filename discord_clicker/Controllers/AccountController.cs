using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using discord_clicker.ViewModels;
using Microsoft.AspNetCore.Authentication;
using discord_clicker.Models.Person;
using discord_clicker.Services.UserHandler;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace discord_clicker.Controllers;

public class AccountController : Controller
{
    private readonly IUserHandler _userHandler;
    public AccountController(IUserHandler userHandler)
    {
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
        if (!ModelState.IsValid) {
            ModelState.AddModelError("", "Некорректно заполнена форма");
            return View(model);
        }
        #nullable enable
        User? user = await _userHandler.GetUserAuthAsync(name: model.Nickname, password: model.Password);
        if (user != null) {
            await Authenticate(user);
            return RedirectToAction("Index", "Home");
        }
        ModelState.AddModelError("", "Введен неверный логин или пароль");
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
        if (!ModelState.IsValid) {
            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            return View(model);
        }
        bool userExistCheck = await _userHandler.ExistByName(model.Nickname);
        if (!userExistCheck) {
            User user = await _userHandler.CreateUserAsync(model);
            await Authenticate(user);
            return RedirectToAction("Index", "Home");
        }
        ModelState.AddModelError("", "Аккаунт с таким логином уже существует");
        return RedirectToAction("Login", "Account");
    }

    private async Task Authenticate(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, Convert.ToString(user.Id)),
            new(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name!)
        };
        ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", 
            ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }
}