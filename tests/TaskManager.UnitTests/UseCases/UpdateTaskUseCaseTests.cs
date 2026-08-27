using Moq;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
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

        var dueDate = DateTime.UtcNow.AddDays(7);

        var request = new UpdateTaskRequest
        {
            Title = "Tarefa atualizada",
            Description = "Nova descrição",
            DueDate = dueDate
        };

        var repositoryMock = new Mock<ITaskRepository>();

        repositoryMock
            .Setup(repository => repository.GetByIdAsync(taskItem.Id, _userId))
            .ReturnsAsync(taskItem);

        var currentUserMock = new Mock<ICurrentUser>();

        currentUserMock
            .Setup(currentUser => currentUser.UserId)
            .Returns(_userId);

        var useCase = new UpdateTaskUseCase(
            repositoryMock.Object,
            currentUserMock.Object);

        var result = await useCase.ExecuteAsync(taskItem.Id, request);

        Assert.Equal("Tarefa atualizada", result!.Title);
        Assert.Equal("Nova descrição", result.Description);
        Assert.Equal(dueDate, result.DueDate);

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
            .Setup(repository => repository.GetByIdAsync(id, _userId))
            .ReturnsAsync((TaskItem?)null);

        var currentUserMock = new Mock<ICurrentUser>();

        currentUserMock
            .Setup(currentUser => currentUser.UserId)
            .Returns(_userId);

        var useCase = new UpdateTaskUseCase(
            repositoryMock.Object,
            currentUserMock.Object);

        var result = await useCase.ExecuteAsync(id, request);

        Assert.Null(result);

        repositoryMock.Verify(
            repository => repository.UpdateAsync(It.IsAny<TaskItem>()),
            Times.Never);
    }
}