using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.UseCases.CreateTask;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly CreateTaskUseCase _createTaskUseCase;

    public TasksController(CreateTaskUseCase createTaskUseCase)
    {
        _createTaskUseCase = createTaskUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
    {
        var taskItem = await _createTaskUseCase.Execute(request);

        return Created(string.Empty, taskItem);
    }
}
