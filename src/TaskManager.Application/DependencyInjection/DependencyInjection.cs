using TaskManager.Application.UseCases.CreateTask;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.UseCases.ListTasks;
using TaskManager.Application.UseCases.GetTaskById;
using TaskManager.Application.UseCases.UpdateTask;
using TaskManager.Application.UseCases.DeleteTask;
using TaskManager.Application.UseCases.StartTask;
using TaskManager.Application.UseCases.CompleteTask;
using TaskManager.Application.UseCases.CreateUser;


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
        services.AddScoped<StartTaskUseCase>();
        services.AddScoped<CompleteTaskUseCase>();
        services.AddScoped<CreateUserUseCase>();
        return services;
    }
}
