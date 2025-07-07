using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data.Context;
using Presentation.Models;
using System.Linq;
using CampusCollabFPI.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Presentation.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly EmployeeAppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILogger<HomeController> logger, EmployeeAppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var isStudent = user != null && await _userManager.IsInRoleAsync(user, "Student");

        var trendingGroups = await _context.Groups
            .Select(g => new TrendingGroupViewModel
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.CourseName,
                MemberCount = g.Members.Count,
                LeaderName = g.Members
                    .Select(m => m.User.UserName)
                    .FirstOrDefault()
            })
            .OrderByDescending(g => g.MemberCount)
            .Take(3)
            .ToListAsync();

        var model = new DashboardViewModel
        {
            ActiveGroups = await _context.Groups.CountAsync(),
            GroupLeaders = await _context.GroupMemberships.Where(m => m.IsAdmin).Select(m => m.UserId).Distinct().CountAsync(),
            TotalStudents = await _userManager.GetUsersInRoleAsync("Student").ContinueWith(t => t.Result.Count),
            MyGroups = isStudent
                ? await _context.GroupMemberships.CountAsync(m => m.UserId == user.Id)
                : await _context.Groups.Select(g => g.Department).Distinct().CountAsync(),
            RecentActivities = await _context.GroupPosts
                .OrderByDescending(p => p.PostedAt)
                .Take(5)
                .Select(p => new ActivityItem
                {
                    Title = "New Post",
                    Description = p.Content,
                    Time = p.PostedAt
                }).ToListAsync(),
            TrendingGroups = trendingGroups
        };

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
