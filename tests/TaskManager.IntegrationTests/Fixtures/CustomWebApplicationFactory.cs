using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Infrastructure.Data;
using TaskManager.IntegrationTests.TestControllers;

namespace TaskManager.IntegrationTests.Fixtures;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                service => service.ServiceType == typeof(DbContextOptions<TaskManagerDbContext>
                ));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            var connectionString = Environment.GetEnvironmentVariable("TEST_CONNECTION_STRING")
                ?? @"Server=(localdb)\MSSQLLocalDB;Database=TaskManager_IntegrationTests;Trusted_Connection=True;TrustServerCertificate=True;";

            services.AddDbContext<TaskManagerDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services
                .AddControllers()
                .AddApplicationPart(typeof(ExceptionTestController).Assembly);

            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<TaskManagerDbContext>();

            dbContext.Database.EnsureCreated();
        });
    }

    public async Task ResetDatabaseAsync()
    {
        using var scope = Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TaskManagerDbContext>();

        dbContext.TaskItems.RemoveRange(dbContext.TaskItems);
        dbContext.Users.RemoveRange(dbContext.Users);

        await dbContext.SaveChangesAsync();
    }
}
