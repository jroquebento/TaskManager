using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.UseCases.CreateTask;

public class CreateTaskUseCase
{
    private readonly ITaskRepository _taskRepository;
    public CreateTaskUseCase(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskItem> Execute(CreateTaskRequest request)
    {
        TaskItem taskItem = new TaskItem(request.Title, request.Description, request.DueDate);
        await _taskRepository.AddAsync(taskItem);
        return taskItem;
    }
}
