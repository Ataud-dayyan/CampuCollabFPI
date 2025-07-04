using Data.Context;
using Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CampusCollabFPI.Data.Models;

namespace Presentation.Controllers
{
    public class GroupController : Controller
    {
        private readonly EmployeeAppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public GroupController(EmployeeAppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //<--------------------------------Index-------------------------------->

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

            var existingMembership = await _context.GroupMemberships
                .FirstOrDefaultAsync(m => m.UserId == currentUser!.Id);

            if (existingMembership != null)
            {
                var userGroup = await _context.Groups
                    .Where(g => g.Id == existingMembership.GroupId)
                    .ToListAsync();
                return View(userGroup);
            }

            var allGroups = await _context.Groups.ToListAsync();
            return View(allGroups);
        }
        //<--------------------------------Create-------------------------------->
        [HttpGet]
        [Authorize(Roles = "Lecturer")]
        public IActionResult Create()
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

            // Save group first
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            // Then add the creator as a member
            var membership = new GroupMembership
            {
                GroupId = group.Id,
                UserId = user.Id
            };

            _context.GroupMemberships.Add(membership);
            await _context.SaveChangesAsync();

            TempData["success"] = "Group created successfully!";
            return RedirectToAction("Index");
        }


        //<--------------------------------Join-------------------------------->

        [HttpPost]
        public async Task<IActionResult> Join(int groupId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var alreadyInGroup = await _context.GroupMemberships
             .AnyAsync(m => m.UserId == user.Id);

            if (alreadyInGroup)
            {
                TempData["Error"] = "You can only join one group.";
                return RedirectToAction("Index");
            }

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

        //<--------------------------------Details-------------------------------->
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Auth");

            // Make sure user is in this group
            var isMember = await _context.GroupMemberships
                .AnyAsync(m => m.GroupId == id && m.UserId == user.Id);

            if (!isMember)
            {
                TempData["Error"] = "You are not a member of this group.";
                return RedirectToAction("Index");
            }

            var group = await _context.Groups
                .Include(g => g.CreatedByUser)
                .Include(g => g.Members).ThenInclude(m => m.User)
                .Include(g => g.Posts).ThenInclude(p => p.User)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
                return NotFound();

            return View(group);
        }

       

        //<--------------------------------PostMessages-------------------------------->

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostMessage(int groupId, string content)
        {
            var userId = _userManager.GetUserId(User);

            var isMember = await _context.GroupMemberships
                .AnyAsync(m => m.GroupId == groupId && m.UserId == userId);

            if (!isMember)
            {
                TempData["Error"] = "You can't post to a group you're not a member of.";
                return RedirectToAction("Index");
            }

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


        //<--------------------------------Leave-------------------------------->

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

            return RedirectToAction("Index");
        }


        //<--------------------------------Delete-------------------------------->

        [Authorize]
        [Authorize(Roles = "Lecturer")]
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
