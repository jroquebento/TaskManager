using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.UseCases.CreateTask;
using TaskManager.Application.UseCases.GetTaskById;
using TaskManager.Application.UseCases.ListTasks;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly CreateTaskUseCase _createTaskUseCase;
    private readonly ListTasksUseCase _listTasksUseCase;
    private readonly GetTaskByIdUseCase _getTaskByIdUseCase;

    public TasksController(CreateTaskUseCase createTaskUseCase, ListTasksUseCase listTasksUseCase, GetTaskByIdUseCase getTaskByIdUseCase)
    {
        _createTaskUseCase = createTaskUseCase;
        _listTasksUseCase = listTasksUseCase;
        _getTaskByIdUseCase = getTaskByIdUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
    {
        var taskItem = await _createTaskUseCase.Execute(request);

        return Created(string.Empty, taskItem);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _listTasksUseCase.ExecuteAsync();

        return Ok(tasks);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var taskItem = await _getTaskByIdUseCase.ExecuteAsync(id);
        if (taskItem == null)
        {
            return NotFound();
        }

        return Ok(taskItem);
    }
}
