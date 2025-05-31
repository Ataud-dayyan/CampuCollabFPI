using Application.ContractMapping;
using Application.Dtos;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Department;

public class DepartmentService : IDepartmentService
{
    private readonly EmployeeAppDbContext _context;

    public DepartmentService(EmployeeAppDbContext context)
    {
        _context = context;
    }

    public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto dto)
    {
        var data = new CreateDepartmentDto
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description
        };

        var department = data.ToModel();

        try
        {
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();

            return department.ToDto();
        }
        catch(Exception ex)
        {
            Console.WriteLine("An error occurred while creating the department.", ex);
            return new DepartmentDto();
        }
    }

    public Task DeleteDepartmentAsync(Guid departmentId)
    {
        throw new NotImplementedException();
    }

    public async Task<DepartmentsDto> GetAllDepartmentsAsync()
    {
        var departments = await _context.Departments.ToListAsync();

        return departments.DepartmentsDto();
    }

    public Task<DepartmentDto> GetDepartmentByIdAsync(Guid departmentId)
    {
        throw new NotImplementedException();
    }

    public Task<DepartmentDto> UpdateDepartmentAsync(DepartmentDto departmentDto)
    {
        throw new NotImplementedException();
    }
}
