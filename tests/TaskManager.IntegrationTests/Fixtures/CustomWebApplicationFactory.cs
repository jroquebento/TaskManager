using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Infrastructure.Data;

namespace TaskManager.IntegrationTests.Fixtures;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                service => service.ServiceType == typeof(DbContextOptions<TaskManagerDbContext>
                ));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<TaskManagerDbContext>(options =>
            {
                options.UseSqlServer(
                    @"Server=(localdb)\MSSQLLocalDB;Database=TaskManager_IntegrationTests;Trusted_Connection=True;TrustServerCertificate=True;");
            });
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
