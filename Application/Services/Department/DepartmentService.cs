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

    public async Task DeleteDepartmentAsync(Guid departmentId)
    {
        try
        {
            var department = await _context.Departments.FindAsync(departmentId);

            if (department == null)
            {
                throw new KeyNotFoundException($"Department with ID {departmentId} not found.");
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while deleting the department.", ex);
        }
    }

    public async Task<DepartmentsDto> GetAllDepartmentsAsync()
    {
        var departments = await _context.Departments
                            .Include(x => x.Employees)
                            .ToListAsync();

        return departments.DepartmentsDto();
    }

    public async Task<DepartmentDto?> GetDepartmentByIdAsync(Guid departmentId)
    {
        var department = await _context.Departments
            .Where(d => d.Id == departmentId)
            .Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Employees = d.Employees.Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email
                }).ToList(),
                EmployeeCount = d.Employees.Count
            })
            .FirstOrDefaultAsync();

        return department;
    }

    public async Task<UpdateDepartmentDto> UpdateDepartmentAsync(UpdateDepartmentDto departmentDto)
    {
        try
        {
            var department = await _context.Departments.FindAsync(departmentDto.Id);

            if (department == null)
            {
                throw new KeyNotFoundException($"Department with ID {departmentDto.Id} not found.");
            }

            department.Name = departmentDto.Name;
            department.Description = departmentDto.Description;

            _context.Departments.Update(department);
            await _context.SaveChangesAsync();

            return department.ToUpdateDto();
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while updating the department.", ex);
            return new UpdateDepartmentDto();
        }
    }
}