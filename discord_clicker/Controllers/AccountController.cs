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

namespace discord_clicker.Controllers;

public class AccountController : Controller
{
    private readonly UserHandler _userHandler;
    public AccountController(UserHandler userHandler)
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
        UserModel user = await _userHandler.GetUser(name: model.Nickname, password: model.Password);
        if (user != null) {
            await Authenticate(user);
            return RedirectToAction("Index", "Home");
        }
        else {
            ModelState.AddModelError("", "Введен неверный логин или пароль");
            return View(model);
        }
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
        bool userExistCheck = await _userHandler.ExistCheck(model.Nickname);
        if (!userExistCheck) {
            User user = await _userHandler.Create(nickname: model.Nickname, password: model.Password, money: 0, 
                clickCoefficient: 1, passiveCoefficient: 0, playStartDate: DateTime.UtcNow );
            UserModel userModel = user.ToViewModel();
            await Authenticate(userModel);
            return RedirectToAction("Index", "Home");
        }
        else {
            ModelState.AddModelError("", "Аккаунт с таким логином уже существует");
            return RedirectToAction("Login", "Account");
        }
    }

    private async Task Authenticate(UserModel user)
    {
        var claims = new List<Claim>{new Claim(ClaimsIdentity.DefaultNameClaimType, Convert.ToString(user.Id))};
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