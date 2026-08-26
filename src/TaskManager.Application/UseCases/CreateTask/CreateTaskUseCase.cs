using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.UseCases.CreateTask;

public class CreateTaskUseCase
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICurrentUser _currentUser;
    public CreateTaskUseCase(ITaskRepository taskRepository, ICurrentUser currentUser)
    {
        _taskRepository = taskRepository;
        _currentUser = currentUser;
    }

    public async Task<TaskItem> Execute(CreateTaskRequest request)
    {
        TaskItem taskItem = new TaskItem(_currentUser.UserId ,request.Title, request.Description, request.DueDate);
        await _taskRepository.AddAsync(taskItem);
        return taskItem;
    }
}
