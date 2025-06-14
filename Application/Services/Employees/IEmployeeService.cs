using Application.Dtos;

namespace Application.Services.Employees;

public interface IEmployeeService
{
    Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto);
    Task<EmployeeDto?> GetEmployeeByIdAsync(Guid employeeId);
    Task<EmployeesDto?> GetAllEmployeesAsync();
    Task UpdateEmployeeAsync(UpdateEmployeeDto dto);
    Task DeleteEmployeeAsync(Guid employeeId);
    Task<EmployeesDto> GetEmployeesByDepartmentIdAsync(Guid departmentId);
}
