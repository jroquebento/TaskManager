using Moq;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Application.UseCases.GetTaskById;
using TaskManager.Application.UseCases.UpdateTask;
using TaskManager.Domain.Entities;

namespace TaskManager.UnitTests.UseCases;

public class UpdateTaskUseCaseTests
{
    private readonly Guid _userId = Guid.NewGuid();
    [Fact]
    public async Task ExecuteAsync_ShouldUpdateTaskWhenTaskExists() 
    {
        var taskItem = new TaskItem(_userId, "Tarefa original", null, null);
        var request = new UpdateTaskRequest
        {
            Title = "Tarefa atualizada",
            Description = "Nova descrição",
            DueDate = new DateTime(2026, 05, 01)
        };

        var repositoryMock = new Mock<ITaskRepository>();

        repositoryMock
            .Setup(repository => repository.GetByIdAsync(taskItem.Id))
            .ReturnsAsync(taskItem);

        var useCase = new UpdateTaskUseCase(repositoryMock.Object);

        var result = await useCase.ExecuteAsync(taskItem.Id, request);

        Assert.Equal("Tarefa atualizada", result!.Title);
        Assert.Equal("Nova descrição", result.Description);
        Assert.Equal(new DateTime(2026, 05, 01), result.DueDate);

        repositoryMock.Verify(
                repository => repository.UpdateAsync(taskItem),
                Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNullWhenTaskDoesNotExist()
    {
        var id = Guid.NewGuid();

        var request = new UpdateTaskRequest
        {
            Title = "Tarefa atualizada",
            Description = "Nova descrição",
            DueDate = new DateTime(2026, 05, 01)
        };

        var repositoryMock = new Mock<ITaskRepository>();
        repositoryMock
            .Setup(repository => repository.GetByIdAsync(id))
            .ReturnsAsync((TaskItem?)null);

        var useCase = new UpdateTaskUseCase(repositoryMock.Object);

        var result = await useCase.ExecuteAsync(id, request);

        Assert.Null(result);

        repositoryMock.Verify(
                repository => repository.UpdateAsync(It.IsAny<TaskItem>()),
                Times.Never);
    }
}
