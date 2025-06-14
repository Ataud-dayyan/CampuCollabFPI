using Application.Dtos;
using Presentation.Models;

namespace Presentation.DtoMapping;

public static class Mapperly
{
    // DepartmentViewModel <-> DepartmentDto
    public static DepartmentViewModel ToViewModel(this DepartmentDto dto)
    {
        return new DepartmentViewModel()
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            EmployeeCount = dto.EmployeeCount,
            Employees = dto.Employees.Select(e => new DepartmentEmployeeViewModel
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email
            }).ToList()
        };
    }

    public static DepartmentDto ToDto(this DepartmentViewModel vm)
    {
        return new DepartmentDto()
        {
            Id = vm.Id,
            Name = vm.Name,
            Description = vm.Description,
            Employees = []
        };
    }

    // DepartmentsViewModel <-> DepartmentsDto
    public static DepartmentsViewModel ToViewModel(this DepartmentsDto dto)
    {
        return new DepartmentsViewModel()
        {
            Departments = dto.Departments.Select(d => d.ToViewModel()).ToList()
        };
    }

    public static DepartmentsDto ToDto(this DepartmentsViewModel vm)
    {
        return new DepartmentsDto()
        {
            Departments = vm.Departments.Select(d => d.ToDto()).ToList()
        };
    }

    public static UpdateDepartmentViewModel ToViewModel(this UpdateDepartmentDto dto)
    {
        return new UpdateDepartmentViewModel()
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description
        };
    }

    public static UpdateDepartmentViewModel ToUpdateDepartmentViewModel(this DepartmentDto dto)
    {
        return new UpdateDepartmentViewModel()
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description
        };
    }

    public static UpdateDepartmentDto ToDto(this UpdateDepartmentViewModel vm)
    {
        return new UpdateDepartmentDto()
        {
            Id = vm.Id,
            Name = vm.Name,
            Description = vm.Description
        };
    }
}
