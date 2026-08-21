using TaskManager.Application.UseCases.CreateTask;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.UseCases.ListTasks;
using TaskManager.Application.UseCases.GetTaskById;
using TaskManager.Application.UseCases.UpdateTask;
using TaskManager.Application.UseCases.DeleteTask;


namespace TaskManager.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateTaskUseCase>();
        services.AddScoped<ListTasksUseCase>();
        services.AddScoped<GetTaskByIdUseCase>();
        services.AddScoped<UpdateTaskUseCase>();
        services.AddScoped<DeleteTaskUseCase>();
        return services;
    }
}
