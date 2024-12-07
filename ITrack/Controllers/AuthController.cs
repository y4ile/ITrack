/*
 *      @Author: yaile
 */

using Microsoft.AspNetCore.Mvc;
using ITrack.Data.ViewModels;
using ITrack.Service.Interfaces;

namespace ITrack.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.CreateUser(model);
                if (response.StatusCode == Data.Enum.StatusCode.OK)
                    return RedirectToAction("Index", "Home");
                
                return Problem(statusCode: (int)response.StatusCode);
            }
            
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.PasswordSignIn(
                    model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded) return RedirectToLocal(returnUrl);
                else ModelState.AddModelError("", "Неверная попытка входа");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _userService.SignOutUser();
            return RedirectToAction("Index", "Home");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
            else return RedirectToAction("Index", "Home");
        }
    }
}
