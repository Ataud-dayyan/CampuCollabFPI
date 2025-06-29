using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Application.Services.Email;

namespace Presentation.Controllers;

public class AuthController : BaseController
{
    private readonly IEmailService _emailService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AuthController(
        SignInManager<IdentityUser> signinManager,
        UserManager<IdentityUser> userManager,
        IEmailService emailService
        )
    {
        _signInManager = signinManager;
        _userManager = userManager;
        _emailService = emailService;
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

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(tempUser);
        var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { userId = tempUser.Id, token }, Request.Scheme);

        try
        {
            await _emailService.SendEmailAsync(model.Email, "Confirm your email", $"Please confirm your account by clicking this link: <a href='{confirmationLink}'>link</a>");
        }
        catch (Exception)
        {
            await _userManager.DeleteAsync(tempUser);
            SetFlashMessage("Failed to send confirmation email. Please try again later.", "error");
            return View(model);
        }

        SetFlashMessage("Registration successful! Please check your email to confirm your account.", "success");
        return RedirectToAction("Login");
    }

    return View(model);
}

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
        {
            SetFlashMessage("Invalid email confirmation link.", "error");
            return RedirectToAction("Login");
        }
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            SetFlashMessage("User not found.", "error");
            return RedirectToAction("Login");
        }
        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            SetFlashMessage("Email confirmed successfully!", "success");
            return RedirectToAction("Login");
        }
        SetFlashMessage("Email confirmation failed.", "error");
        return RedirectToAction("Login");
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
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    SetFlashMessage("Please confirm your email before logging in.", "error");
                    return View(model);
                }

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
