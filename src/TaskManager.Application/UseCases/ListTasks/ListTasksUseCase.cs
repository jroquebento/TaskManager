using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.UseCases.ListTasks;

public class ListTasksUseCase
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICurrentUser _currentUser;

    public ListTasksUseCase(ITaskRepository taskRepository, ICurrentUser currentUser) 
    {
        _taskRepository = taskRepository;
        _currentUser = currentUser;
    }

    public async Task<List<TaskItem>> ExecuteAsync() 
    {
        return await _taskRepository.GetAllByUserIdAsync(_currentUser.UserId);
    }
}
