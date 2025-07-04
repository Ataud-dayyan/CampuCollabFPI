using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Authorize(Roles = "Lecturer")]
    public class LecturerController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }

    [Authorize(Roles = "Student,GroupLeader")]
    public class StudentController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
