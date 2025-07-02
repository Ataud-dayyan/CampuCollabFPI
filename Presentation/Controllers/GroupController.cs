using Data.Context;
using Data.Model;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> Details(int id)
        {
            var group = await _context.Groups
                .Include(g => g.CreatedByUser)
                .Include(g => g.Members).ThenInclude(m => m.User)
                .Include(g => g.Posts).ThenInclude(p => p.User)
                .FirstOrDefaultAsync(g => g.Id == id);



            if (group == null)
                return NotFound();

            return View(group);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GroupModel group)
        {
            if (!ModelState.IsValid)
            {
                return View(group);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            group.CreatedById = user.Id;
            group.CreatedAt = DateTime.Now;

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            TempData["success"] = "Group created successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostMessage(int groupId, string content)
        {
            var userId = _userManager.GetUserId(User);

            var post = new GroupPost
            {
                GroupId = groupId,
                UserId = userId,
                Content = content
            };

            _context.GroupPosts.Add(post);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = groupId });
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Leave(int groupId)
        {
            var userId = _userManager.GetUserId(User);

            var membership = await _context.GroupMemberships
                .FirstOrDefaultAsync(m => m.GroupId == groupId && m.UserId == userId);

            if (membership != null)
            {
                _context.GroupMemberships.Remove(membership);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = groupId });
        }


        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            var currentUserId = _userManager.GetUserId(User);

            var isAdmin = await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(User), "Admin");

            if (group == null || (group.CreatedById != currentUserId && !isAdmin))
                return Unauthorized();

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

            if (group.CreatedById != currentUserId && !isAdmin)
            {
                return Unauthorized();
            }

        }


    }
}
