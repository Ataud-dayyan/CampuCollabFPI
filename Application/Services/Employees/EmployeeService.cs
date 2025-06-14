using Application.Dtos;
using Data.Context;
using Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Employees;

public class EmployeeService : IEmployeeService
{
    private readonly EmployeeAppDbContext _context;

    public EmployeeService(EmployeeAppDbContext context)
    {
        _context = context;
    }

    public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto)
    {
        var data = new Employee
        {
            Id = Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            HireDate = dto.HireDate,
            Salary = dto.Salary,
            DepartmentId = dto.DepartmentId
        };

        await _context.Employees.AddAsync(data);
        await _context.SaveChangesAsync();

        return new EmployeeDto
        {
            Id = data.Id,
            FirstName = data.FirstName,
            LastName = data.LastName,
            Email = data.Email,
            HireDate = data.HireDate,
            Salary = data.Salary,
            DepartmentId = data.DepartmentId
        };
    }

    public async Task DeleteEmployeeAsync(Guid employeeId)
    {
        var employee = await _context.Employees.FindAsync(employeeId);

        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");
        }

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }

    public async Task<EmployeesDto?> GetAllEmployeesAsync()
    {
        var data = await _context.Employees.Select(e => new EmployeeDto
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            HireDate = e.HireDate,
            Salary = e.Salary,
            DepartmentId = e.DepartmentId,
            DepartmentName = e.Department.Name
        }).ToListAsync();

        return new EmployeesDto
        {
            Employees = data
        };
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(Guid employeeId)
    {
        return await _context.Employees
            .Where(e => e.Id == employeeId)
            .Select(e => new EmployeeDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                HireDate = e.HireDate,
                Salary = e.Salary,
                DepartmentId = e.DepartmentId
            })
            .FirstOrDefaultAsync();
    }

    public Task<EmployeesDto> GetEmployeesByDepartmentIdAsync(Guid departmentId)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateEmployeeAsync(UpdateEmployeeDto dto)
    {
        var employee = await _context.Employees.FindAsync(dto.Id);

        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {dto.Id} not found.");
        }

        employee.FirstName = dto.FirstName;
        employee.LastName = dto.LastName;
        employee.Email = dto.Email;
        employee.HireDate = dto.HireDate;
        employee.Salary = dto.Salary;
        employee.DepartmentId = dto.DepartmentId;

        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
    }
}
