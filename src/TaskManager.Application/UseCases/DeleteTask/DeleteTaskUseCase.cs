using TaskManager.Application.Interfaces;

namespace TaskManager.Application.UseCases.DeleteTask;

public class DeleteTaskUseCase
{
    private readonly ITaskRepository _taskRepository;

    public DeleteTaskUseCase(ITaskRepository taskRepository)
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
        await _taskRepository.DeleteAsync(taskItem);

        return true;
    }
}
