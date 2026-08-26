using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.UseCases.UpdateTask;

public class UpdateTaskUseCase
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICurrentUser _currentUser;

    public UpdateTaskUseCase(ITaskRepository taskRepository, ICurrentUser currentUser)
    {
        _taskRepository = taskRepository;
        _currentUser = currentUser;
    }

    public async Task<TaskItem?> ExecuteAsync(Guid id, UpdateTaskRequest request)
    {
        var taskItem = await _taskRepository.GetByIdAsync(id, _currentUser.UserId);

        if (taskItem == null)
        {
            return null;
        }

        taskItem.Update(request.Title, request.Description, request.DueDate);

        await _taskRepository.UpdateAsync(taskItem);

        return taskItem;
    }
}
