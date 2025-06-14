using Application.Dtos;
using Application.Services.Department;
using Application.Services.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation.Models.EmployeeVM;

namespace Presentation.Controllers;

public class EmployeeController : BaseController
{
    private readonly IEmployeeService _employeeService;
    private readonly IDepartmentService _departmentService;

    public EmployeeController(
        IEmployeeService employeeService,
        IDepartmentService departmentService)
    {
        _employeeService = employeeService;
        _departmentService = departmentService;
    }

    public async Task<IActionResult> Index()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();

        var model = new EmployeesViewModel
        {
            Employees = employees?.Employees.Select(e => new EmployeeViewModel
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Department = e.DepartmentName
            }).ToList()!
        };

        return View(model);
    }

    public async Task<IActionResult> Create()
    {
        var model = new CreateEmployeeViewModel
        {
            HireDate = DateTime.Today,
            Departments = await GetDepartmentSelectList()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEmployeeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.HireDate = DateTime.Today;
            model.Departments = await GetDepartmentSelectList();
            return View(model);
        }

        var dto = new CreateEmployeeDto
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            HireDate = model.HireDate,
            Salary = model.Salary,
            DepartmentId = model.DepartmentId
        };

        var response = await _employeeService.CreateEmployeeAsync(dto);

        if (response == null)
        {
            SetFlashMessage("Failed to create employee.", "error");
            model.HireDate = DateTime.Today;
            model.Departments = await GetDepartmentSelectList();
            return View(model);
        }

        SetFlashMessage("Employee created successfully.", "success");

        return RedirectToAction(nameof(Index));
    }

    private async Task<IEnumerable<SelectListItem>> GetDepartmentSelectList()
    {
        var data = await _departmentService.GetAllDepartmentsAsync();

        return data.Departments.Select(d => new SelectListItem
        {
            Value = d.Id.ToString(),
            Text = d.Name
        });
    }
}
