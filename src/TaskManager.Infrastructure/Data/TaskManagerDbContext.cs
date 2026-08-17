using Microsoft.EntityFrameworkCore;

namespace TaskManager.Infrastructure.Data;

internal class TaskManagerDbContext : DbContext
{
    public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : base(options)
    {
        
    }
}
