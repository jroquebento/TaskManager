using TaskManager.Application.UseCases.CreateTask;
using Microsoft.Extensions.DependencyInjection;


namespace TaskManager.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateTaskUseCase>();
        return services;
    }
}
