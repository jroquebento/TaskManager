using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.UseCases.UpdateTask;

public class UpdateTaskUseCase
{
    private readonly ITaskRepository _taskRepository;
    public UpdateTaskUseCase(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskItem?> ExecuteAsync(Guid id, UpdateTaskRequest request)
    {
        var taskItem = await _taskRepository.GetByIdAsync(id);
        if (taskItem == null)
        {
            return null;
        }
        taskItem.Update(request.Title, request.Description, request.DueDate);
        await _taskRepository.UpdateAsync(taskItem);

        return taskItem;
    }
}
