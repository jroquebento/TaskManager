using TaskManager.Application.Interfaces;

namespace TaskManager.Application.UseCases.StartTask;

public class StartTaskUseCase
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICurrentUser _currentUser;

    public StartTaskUseCase(
        ITaskRepository taskRepository,
        ICurrentUser currentUser)
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

        taskItem.Start();
        await _taskRepository.UpdateAsync(taskItem);

        return true;
    }
}
