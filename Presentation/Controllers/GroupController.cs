using CampusCollabFPI.Data.Models;
using Data.Context;
using Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Models;
using System.Text.RegularExpressions;

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
            if (currentUser == null)
            {
                TempData["Error"] = "You must be logged in to view groups.";
                return RedirectToAction("Login", "Account");
            }

            var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");
            var isLecturer = await _userManager.IsInRoleAsync(currentUser, "Lecturer");
            if (isLecturer)
            {
                var allStudents = await _userManager.GetUsersInRoleAsync("Student");
                ViewBag.AllStudents = allStudents.ToList();
            }

            if (!isAdmin && !isLecturer)
            {
                var existingMembership = await _context.GroupMemberships
                    .FirstOrDefaultAsync(m => m.UserId == currentUser.Id);

                if (existingMembership != null)
                {
                    var userGroup = await _context.Groups
                        .Where(g => g.Id == existingMembership.GroupId)
                        .Include(g => g.Members)
                        .ToListAsync();

                    return View(userGroup);
                }
                else
                {
                    return View(new List<Group>());
                }
            }

            var allGroups = await _context.Groups
                .Include(g => g.Members)
                .ToListAsync();
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
            //var membership = new GroupMembership
            //{
            //    GroupId = group.Id,
            //    UserId = user.Id
            //};

            //_context.GroupMemberships.Add(membership);
            //await _context.SaveChangesAsync();

            TempData["success"] = "Group created successfully!";
            return RedirectToAction("Index");
        }


        //<--------------------------------Join-------------------------------->

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(int groupId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            // Only restrict students to one group
            var isStudent = await _userManager.IsInRoleAsync(user, "Student");
            if (isStudent)
            {
                var alreadyInGroup = await _context.GroupMemberships
                    .AnyAsync(m => m.UserId == user.Id);

                if (alreadyInGroup)
                {
                    TempData["Error"] = "You can only join one group. Leave your current group to join another.";
                    return RedirectToAction("Index");
                }
            }

            // Add new membership for any user (students, lecturers, etc.)
            var membership = new GroupMembership
            {
                GroupId = groupId,
                UserId = user.Id
            };

            _context.GroupMemberships.Add(membership);
            await _context.SaveChangesAsync();

            TempData["Success"] = "You have successfully joined the group!";
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

            // Add this: Get all students for the dropdown if user is a lecturer
            var isLecturer = await _userManager.IsInRoleAsync(user, "Lecturer");
            if (isLecturer)
            {
                var allStudents = await _userManager.GetUsersInRoleAsync("Student");
                ViewBag.AllStudents = allStudents.ToList();
            }

            return View(group);
        }

        //<--------------------------------AddMembers-------------------------------->
        [HttpPost]
        [Authorize(Roles = "Lecturer")]
        public async Task<IActionResult> AddStudent(int groupId, string studentId)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group == null) return NotFound();

            var isAlreadyMember = await _context.GroupMemberships
                .AnyAsync(m => m.GroupId == groupId && m.UserId == studentId);

            if (!isAlreadyMember)
            {
                _context.GroupMemberships.Add(new GroupMembership
                {
                    GroupId = groupId,
                    UserId = studentId
                });
                await _context.SaveChangesAsync();
                TempData["success"] = "Student added to group!";
            }
            else
            {
                TempData["Error"] = "Student is already a member of this group.";
            }

            return RedirectToAction("Details", new { id = groupId });
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

        }

        //<--------------------------------Remove Students-------------------------------->
        [HttpPost]
        [Authorize(Roles = "Lecturer")]
        public async Task<IActionResult> RemoveStudent(int groupId, string userId)
        {
            var group = await _context.Groups
                .Include(g => g.Members)
                .FirstOrDefaultAsync(g => g.Id == groupId);

            if (group == null || string.IsNullOrEmpty(userId))
            {
                TempData["PostNotification"] = "❌ Invalid group or user.";
                return RedirectToAction("Details", new { id = groupId });
            }

            var membership = group.Members.FirstOrDefault(m => m.UserId == userId);
            if (membership != null)
            {
                group.Members.Remove(membership);
                await _context.SaveChangesAsync();
                TempData["PostNotification"] = "✅ Student removed successfully.";
            }
            else
            {
                TempData["PostNotification"] = "⚠️ Membership not found.";
            }

            return RedirectToAction("Details", new { id = groupId });
        }



        [HttpGet]
        public async Task<IActionResult> StudentList()
        {
            // Get all users in the Student role
            var students = await _userManager.GetUsersInRoleAsync("Student");

            // Get all group memberships for students
            var memberships = await _context.GroupMemberships
                .Include(m => m.Group)
                .Where(m => students.Select(s => s.Id).Contains(m.UserId))
                .ToListAsync();

            // Convert to view models
            var studentViewModels = students.Select(student =>
            {
                var membership = memberships.FirstOrDefault(m => m.UserId == student.Id);
                var currentGroupName = membership?.Group?.Name ?? string.Empty;

                return new ProfileViewModel
                {
                    UserName = student.UserName ?? string.Empty,
                    Email = student.Email ?? string.Empty,
                    StudentId = student.StudentId ?? string.Empty,
                    CurrentGroupName = currentGroupName,
                    SelectedAvatar = student.Avatar
                };
            }).ToList();

            return View(studentViewModels);
        }

    }
}
