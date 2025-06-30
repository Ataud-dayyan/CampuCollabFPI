using Microsoft.Extensions.DependencyInjection;

namespace Application;
public static class ApplicationServiceExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services;
    }
}
