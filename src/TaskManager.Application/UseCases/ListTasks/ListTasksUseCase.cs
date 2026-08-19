using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.UseCases.ListTasks;

public class ListTasksUseCase
{
    private readonly ITaskRepository _taskRepository;

    public ListTasksUseCase(ITaskRepository taskRepository) 
    {
        _taskRepository = taskRepository;
    }

    public async Task<List<TaskItem>> ExecuteAsync() 
    {
        return await _taskRepository.GetAllAsync();
    }
}
