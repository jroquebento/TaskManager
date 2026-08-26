using TaskManager.Application.Interfaces;

namespace TaskManager.Application.UseCases.CompleteTask;

public class CompleteTaskUseCase
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICurrentUser _currentUser;
    public CompleteTaskUseCase(
        ITaskRepository taskRepository,
        ICurrentUser currentUser)
    {
        _taskRepository = taskRepository;
        _currentUser = currentUser;
    }

    public async Task<bool> ExecuteAsync(Guid id)
    {
        var taskItem = await _taskRepository.GetByIdAsync(
        id,
        _currentUser.UserId);

        if (taskItem == null)
        {
            return false;
        }

        taskItem.Complete();
        await _taskRepository.UpdateAsync(taskItem);

        return true;
    }
}
