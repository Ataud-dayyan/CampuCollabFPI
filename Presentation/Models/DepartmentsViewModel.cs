namespace Presentation.Models;

public class DepartmentViewModel
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public int EmployeeCount { get; set; }
    public List<DepartmentEmployeeViewModel> Employees { get; set; } = default!;
}

public  class DepartmentEmployeeViewModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
}

public class DepartmentsViewModel
{
    public List<DepartmentViewModel> Departments { get; set; } = default!;
}
