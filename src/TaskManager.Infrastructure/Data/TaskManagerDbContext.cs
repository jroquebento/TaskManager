using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Data;

public class TaskManagerDbContext : DbContext
{
    public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : base(options)
    { }

    public DbSet<TaskItem> TaskItems { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
         {
             entity.HasKey(user => user.Id);

             entity.Property(user => user.Name)
             .IsRequired()
             .HasMaxLength(100);

             entity.Property(user => user.Email)
             .IsRequired()
             .HasMaxLength(200);

             entity.Property(user => user.PasswordHash)
             .IsRequired();

             entity.HasIndex(user => user.Email)
             .IsUnique();
         });

        modelBuilder.Entity<User>()
            .HasMany(user => user.TaskItems)
            .WithOne(taskItem => taskItem.User)
            .HasForeignKey(taskItem => taskItem.UserId)
            .IsRequired();
    }
}
