using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.Mappers;
using TaskManager.Application.UseCases.CompleteTask;
using TaskManager.Application.UseCases.CreateTask;
using TaskManager.Application.UseCases.DeleteTask;
using TaskManager.Application.UseCases.GetTaskById;
using TaskManager.Application.UseCases.ListTasks;
using TaskManager.Application.UseCases.StartTask;
using TaskManager.Application.UseCases.UpdateTask;
using TaskManager.Domain.Entities;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly CreateTaskUseCase _createTaskUseCase;
    private readonly ListTasksUseCase _listTasksUseCase;
    private readonly GetTaskByIdUseCase _getTaskByIdUseCase;
    private readonly UpdateTaskUseCase _updateTaskUseCase;
    private readonly DeleteTaskUseCase _deleteTaskUseCase;
    private readonly StartTaskUseCase _startTaskUseCase;
    private readonly CompleteTaskUseCase _completeTaskUseCase;

    public TasksController(CreateTaskUseCase createTaskUseCase, ListTasksUseCase listTasksUseCase, GetTaskByIdUseCase getTaskByIdUseCase, UpdateTaskUseCase updateTaskUseCase, DeleteTaskUseCase deleteTaskUseCase, StartTaskUseCase startTaskUseCase, CompleteTaskUseCase completeTaskUseCase )
    {
        _createTaskUseCase = createTaskUseCase;
        _listTasksUseCase = listTasksUseCase;
        _getTaskByIdUseCase = getTaskByIdUseCase;
        _updateTaskUseCase = updateTaskUseCase;
        _deleteTaskUseCase = deleteTaskUseCase;
        _startTaskUseCase = startTaskUseCase;
        _completeTaskUseCase = completeTaskUseCase;
    }

    [HttpPost]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
    {
        var taskItem = await _createTaskUseCase.Execute(request);

        var response = TaskMapper.ToResponse(taskItem);

        return Created(string.Empty, response);
    }


    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TaskResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _listTasksUseCase.ExecuteAsync();

        var response = tasks.Select(TaskMapper.ToResponse);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var taskItem = await _getTaskByIdUseCase.ExecuteAsync(id);
        if (taskItem == null)
        {
            return NotFound();
        }

        var response = TaskMapper.ToResponse(taskItem);
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskRequest request) 
    {
        var taskItem = await _updateTaskUseCase.ExecuteAsync(id, request);
        
        if (taskItem == null)
        {
            return NotFound();
        }

        var response = TaskMapper.ToResponse(taskItem);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id) 
    {
        var deleted = await _deleteTaskUseCase.ExecuteAsync(id);
        if (!deleted) 
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost("{id:guid}/start")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Start(Guid id) 
    {
        var started = await _startTaskUseCase.ExecuteAsync(id);

        if (!started) 
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpPost("{id:guid}/complete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Complete(Guid id) 
    {
        var completed = await _completeTaskUseCase.ExecuteAsync(id);

        if (!completed) 
        {
            return NotFound();
        }

        return Ok();
    }
}

