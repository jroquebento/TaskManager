using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly TaskManagerDbContext _context;
    public TaskRepository(TaskManagerDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TaskItem taskItem)
    {
        _context.TaskItems.Add(taskItem);
        await _context.SaveChangesAsync();
    }
    public async Task<List<TaskItem>> GetAllByUserIdAsync(Guid userId)
    {
        return await _context.TaskItems
            .Where(task => task.UserId == userId)
            .ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id, Guid userId)
    {
        return await _context.TaskItems
            .FirstOrDefaultAsync(task =>
                task.Id == id &&
                task.UserId == userId);
    }

    public async Task UpdateAsync(TaskItem taskItem)
    {
        _context.TaskItems.Update(taskItem);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(TaskItem taskItem)
    {
        _context.TaskItems.Remove(taskItem);
        await _context.SaveChangesAsync();
    }
}
