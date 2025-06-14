using Application.Services.Department;
using Application.Services.Employees;
using Microsoft.Extensions.DependencyInjection;

namespace Application;
public static class ApplicationServiceExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IEmployeeService, EmployeeService>();

        return services;
    }
}
