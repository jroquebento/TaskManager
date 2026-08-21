using TaskManager.Application.Interfaces;

namespace TaskManager.Application.UseCases.StartTask;

public class StartTaskUseCase
{
    private readonly ITaskRepository _taskRepository;
    public StartTaskUseCase(ITaskRepository taskRepository)
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
        taskItem.Start();
        await _taskRepository.UpdateAsync(taskItem);

        return true;
    }
}
