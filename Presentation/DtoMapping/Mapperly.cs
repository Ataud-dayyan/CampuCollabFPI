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
            Description = dto.Description
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
}
