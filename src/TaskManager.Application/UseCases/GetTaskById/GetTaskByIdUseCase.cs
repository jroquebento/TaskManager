using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.UseCases.GetTaskById;

public class GetTaskByIdUseCase
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICurrentUser _currentUser;
    public GetTaskByIdUseCase(ITaskRepository taskRepository, ICurrentUser currentUser)
    {
        _taskRepository = taskRepository;
        _currentUser = currentUser;
    }

    public async Task<TaskItem?> ExecuteAsync(Guid id)
    {
        return await _taskRepository.GetByIdAsync(
            id,
            _currentUser.UserId);
    }
}
