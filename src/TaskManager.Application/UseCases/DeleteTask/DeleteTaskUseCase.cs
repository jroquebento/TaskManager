using TaskManager.Application.Interfaces;

namespace TaskManager.Application.UseCases.DeleteTask;

public class DeleteTaskUseCase
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICurrentUser _currentUser;

    public DeleteTaskUseCase(ITaskRepository taskRepository, ICurrentUser currentUser)
    {
        _taskRepository = taskRepository;
        _currentUser = currentUser;
    }

    public async Task<bool> ExecuteAsync(Guid id)
    {
        var taskItem = await _taskRepository.GetByIdAsync(id, _currentUser.UserId);

        if (taskItem == null)
        {
            return false;
        }

        await _taskRepository.DeleteAsync(taskItem);

        return true;
    }
}
