using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Presentation.Models;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers;

public class AuthController : BaseController
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AuthController(
        SignInManager<IdentityUser> signinManager,
        UserManager<IdentityUser> userManager
        )
    {
        _signInManager = signinManager;
        _userManager = userManager;
    }

    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var tempUser = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var tempResult = await _userManager.CreateAsync(tempUser, model.Password);

            if (!tempResult.Succeeded)
            {
                var errorMessages = string.Join("<br>", tempResult.Errors.Select(e => e.Description));
                SetFlashMessage(errorMessages, "error");
                return View(model);
            }

            SetFlashMessage("Registration successful! You can now log in.", "success");
            return RedirectToAction("Login");
        }

        return View(model);
    }


    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

            }

           

            SetFlashMessage("Invalid Login attempt!", "error");
        }
        return View(model);
    }

    //[Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
