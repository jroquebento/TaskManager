using TaskManager.Application.UseCases.CreateTask;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.UseCases.ListTasks;
using TaskManager.Application.UseCases.GetTaskById;


namespace TaskManager.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateTaskUseCase>();
        services.AddScoped<ListTasksUseCase>();
        services.AddScoped<GetTaskByIdUseCase>();
        return services;
    }
}
