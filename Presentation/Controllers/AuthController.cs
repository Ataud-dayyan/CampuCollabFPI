using CampusCollabFPI.Data.Models;
using Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Presentation.Models;

namespace Presentation.Controllers;

public class AuthController : BaseController
{
    private readonly EmployeeAppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        EmployeeAppDbContext context
    )
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _context = context;
    }

    // Role selection page
    public IActionResult ChooseRole()
    {
        return View();
    }

    //<---------------------Student registration--------------------->
    public IActionResult RegisterStudent()
    {
        PopulateStudentDepartmentDropdown();
        return View();
    }
    private void PopulateStudentDepartmentDropdown()
    {
        ViewBag.DepartmentList = new SelectList(new[]
        {
            new { Value = "Computer Science", Text = "Computer Science" },
        },
        "Value", "Text");
    }


    [HttpPost]
    public async Task<IActionResult> RegisterStudent(StudentRegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                DisplayName = model.UserName,
                StudentId = model.StudentId 
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Student");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            if (!result.Succeeded)
            {
                var errorMessages = string.Join(" ", result.Errors.Select(e => e.Description));
                SetFlashMessage(errorMessages, "error");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }
    
    //<---------------------Lecturer registration--------------------->
    public IActionResult RegisterLecturer()
    {
        PopulateLecturerDepartmentDropdown();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterLecturer(LecturerRegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                DisplayName = model.UserName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Lecturer");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            if (!result.Succeeded)
            {
                var errorMessages = string.Join(" ", result.Errors.Select(e => e.Description));
                SetFlashMessage(errorMessages, "error");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);

            }
        }
        PopulateLecturerDepartmentDropdown();

        return View(model);
    }
    private void PopulateLecturerDepartmentDropdown()
    {
        ViewBag.DepartmentList = new SelectList(new[]
        {
            new { Value = "Computer Science", Text = "Computer Science" },
        }, 
        "Value", "Text");
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

                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

            }

           

            SetFlashMessage("Invalid Login attempt!", "error");
        }
        return View(model);
    }


    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Auth");

        // Get the user's group if any
        var groupMembership = await _context.GroupMemberships
            .Include(m => m.Group)
            .FirstOrDefaultAsync(m => m.UserId == user.Id);

        var model = new ProfileViewModel
        {
            Email = user.Email!,
            UserName = user.UserName!,
            StudentId = user.StudentId!,
            CurrentGroupName = groupMembership?.Group?.Name ?? "No group",
            AvatarOptions = new List<string> { "Avatar1.jpg", "Avatar2.png", "Avatar3.png", "Avatar4.png", "Avatar5.png", "Avatar6.png", "Avatar7.png", "Avatar8.jpg", "Avatar9.jpg", "Avatar10.jpg", "Avatar11.jpg" }, 
            SelectedAvatar = user.Avatar 
        };

        return View(model);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdateAvatar(string selectedAvatar)
    {
        var user = await _userManager.GetUserAsync(User) as ApplicationUser;
        if (user == null) return RedirectToAction("Login", "Auth");

        user.Avatar = selectedAvatar;
        await _userManager.UpdateAsync(user);

        TempData["success"] = "Avatar updated!";
        return RedirectToAction("Profile");
    }




    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
