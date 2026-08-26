using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces;

public interface ITaskRepository
{
    Task AddAsync(TaskItem taskItem);
    Task<List<TaskItem>> GetAllByUserIdAsync(Guid userId);
    Task<TaskItem?> GetByIdAsync(Guid id, Guid userId);
    Task UpdateAsync(TaskItem taskItem);
    Task DeleteAsync(TaskItem taskItem);
}
