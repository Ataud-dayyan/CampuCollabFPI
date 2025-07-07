using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class StudentRegisterViewModel
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string StudentId { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Department")]
        public string? Department { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required, DataType(DataType.Password), Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Role { get; set; } = "Student";
    }

    public class StudentsViewModel {
        public List<StudentRegisterViewModel> Students { get; set; } = [];
    }
    public class LecturerRegisterViewModel
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Department")]
        public string? Department { get; set; }
        public string Title { get; set; } = string.Empty;
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required, DataType(DataType.Password), Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Role { get; set; } = "Lecturer";
    }
}