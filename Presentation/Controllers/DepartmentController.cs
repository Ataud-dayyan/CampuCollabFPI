using Application.Dtos;
using Application.Services.Department;
using Microsoft.AspNetCore.Mvc;
using Presentation.DtoMapping;
using Presentation.Models;

namespace Presentation.Controllers;

public class DepartmentController : BaseController
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    public async Task<IActionResult> Index()
    {
        var departments = await _departmentService.GetAllDepartmentsAsync();

        var viewModel = departments.ToViewModel();

        return View(viewModel);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var response = await _departmentService.GetDepartmentByIdAsync(id);

        if (response == null)
        {
            SetFlashMessage("Department not found.", "error");
            return RedirectToAction(nameof(Index));
        }

        var viewModel = response.ToViewModel();

        return View(viewModel);
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateDepartmentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var viewModel = new CreateDepartmentDto
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            Description = model.Description
        };

        var result = await _departmentService.CreateDepartmentAsync(viewModel);

        if (result == null)
        {
            SetFlashMessage("An error occurred while creating the department. Please try again.", "error");
            return View(model);
        }

        SetFlashMessage("Department created successfully.", "success");

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var department = await _departmentService.GetDepartmentByIdAsync(id);

        if (department == null)
        {
            SetFlashMessage("Department not found.", "error");
            return RedirectToAction(nameof(Index));
        }

        var viewModel = department.ToUpdateDepartmentViewModel();

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateDepartmentViewModel model)
    {
        var updateDepartment = model.ToDto();

        var response = await _departmentService.UpdateDepartmentAsync(updateDepartment);

        if (response == null)
        {
            SetFlashMessage("An error occurred while updating the department. Please try again.", "error");
            return View(model);
        }

        SetFlashMessage("Department updated successfully.", "success");
        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _departmentService.DeleteDepartmentAsync(id);
        SetFlashMessage("Department deleted successfully.", "success");

        return RedirectToAction(nameof(Index));
    }
}
