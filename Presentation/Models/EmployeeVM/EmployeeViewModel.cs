using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.EmployeeVM;

public class EmployeeViewModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Department { get; set; } = default!;
}

public class EmployeesViewModel
{
    public List<EmployeeViewModel> Employees { get; set; } = [];
}

public class CreateEmployeeViewModel
{
    [Required(ErrorMessage = "First name is required.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = default!;

    [Required(ErrorMessage = "Last name is required.")]
    [Display(Name ="Last Name")]
    public string LastName { get; set; } = default!;

    [Required(ErrorMessage = "Email name is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Hire date is required.")]
    [DataType(DataType.Date)]
    [Display(Name = "Hire Date")]
    public DateTime HireDate { get; set; }

    [Required(ErrorMessage = "Salary is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number.")]
    public decimal Salary { get; set; }

    [Required(ErrorMessage = "Department is required.")]
    [Display(Name = "Department")]
    public Guid DepartmentId { get; set; }

    public IEnumerable<SelectListItem> Departments { get; set; } = default!;
}

public class EmployeeDetailsViewModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public DateTime HireDate { get; set; }
    public decimal Salary { get; set; }
    public string Department { get; set; } = default!;
}

public class UpdateEmployeeViewModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public DateTime HireDate { get; set; }
    public decimal Salary { get; set; }
    public Guid DepartmentId { get; set; }
}
