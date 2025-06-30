using Data.Context;
using Data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers
{
    public class GroupController : Controller
    {
        private readonly EmployeeAppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public GroupController(EmployeeAppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var groups = await _context.Groups.ToListAsync();
            return View(groups);
        }
        [HttpPost]
        public async Task<IActionResult> Join(int groupId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            // Check if already a member
            var existing = await _context.GroupMemberships
                .FirstOrDefaultAsync(m => m.GroupId == groupId && m.UserId == user.Id);

            if (existing == null)
            {
                var membership = new GroupMembership
                {
                    GroupId = groupId,
                    UserId = user.Id,
                };
                _context.GroupMemberships.Add(membership);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Group", new { id = groupId });
        }
    }
}
