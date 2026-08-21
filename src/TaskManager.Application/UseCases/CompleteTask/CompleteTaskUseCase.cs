using TaskManager.Application.Interfaces;

namespace TaskManager.Application.UseCases.CompleteTask;

public class CompleteTaskUseCase
{
    private readonly ITaskRepository _taskRepository;
    public CompleteTaskUseCase(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }
    public async Task<bool> ExecuteAsync(Guid id)
    {
        var taskItem = await _taskRepository.GetByIdAsync(id);
        if (taskItem == null)
        {
            return false;
        }
        taskItem.Complete();
        await _taskRepository.UpdateAsync(taskItem);

        return true;
    }
}
