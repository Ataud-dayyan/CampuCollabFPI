using Application.Dtos;

namespace Application.Services.Department;

public interface IDepartmentService
{
    Task<DepartmentsDto> GetAllDepartmentsAsync();
    Task<DepartmentDto> GetDepartmentByIdAsync(Guid departmentId);
    Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto);
    Task<DepartmentDto> UpdateDepartmentAsync(DepartmentDto departmentDto);
    Task DeleteDepartmentAsync(Guid departmentId);
}
