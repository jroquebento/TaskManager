using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Interfaces;
using TaskManager.Infrastructure.Data;
using TaskManager.Infrastructure.Repositories;
using TaskManager.Infrastructure.Security;

namespace TaskManager.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
        )
    {
        services.AddDbContext<TaskManagerDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}
