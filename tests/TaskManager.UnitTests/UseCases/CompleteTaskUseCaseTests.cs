using Moq;
using TaskManager.Application.Interfaces;
using TaskManager.Application.UseCases.CompleteTask;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Exceptions;

namespace TaskManager.UnitTests.UseCases;

public class CompleteTaskUseCaseTests
{
    private readonly Guid _userId = Guid.NewGuid();

    [Fact]
    public async Task ExecuteAsync_ShouldCompleteTaskWhenTaskExists()
    {
        TaskItem taskItem = new(_userId, "Tarefa 1", null, null);
        taskItem.Start();

        var repositoryMock = new Mock<ITaskRepository>();

        repositoryMock
            .Setup(repository => repository.GetByIdAsync(taskItem.Id))
            .ReturnsAsync(taskItem);

        var useCase = new CompleteTaskUseCase(repositoryMock.Object);

        var result = await useCase.ExecuteAsync(taskItem.Id);

        Assert.True(result);
        Assert.Equal(TaskItemStatus.Completed, taskItem.Status);

        repositoryMock.Verify(
            repository => repository.UpdateAsync(taskItem),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnFalseWhenTaskDoesNotExist()
    {
        var id = Guid.NewGuid();

        var repositoryMock = new Mock<ITaskRepository>();

        repositoryMock
            .Setup(repository => repository.GetByIdAsync(id))
            .ReturnsAsync((TaskItem?)null);

        var useCase = new CompleteTaskUseCase(repositoryMock.Object);

        var result = await useCase.ExecuteAsync(id);

        Assert.False(result);

        repositoryMock.Verify(
            repository => repository.UpdateAsync(It.IsAny<TaskItem>()),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowExceptionWhenTaskIsAlreadyCompleted()
    {
        TaskItem taskItem = new(_userId, "Tarefa 1", null, null);
        taskItem.Start();
        taskItem.Complete();

        var repositoryMock = new Mock<ITaskRepository>();

        repositoryMock
            .Setup(repository => repository.GetByIdAsync(taskItem.Id))
            .ReturnsAsync(taskItem);

        var useCase = new CompleteTaskUseCase(repositoryMock.Object);

        await Assert.ThrowsAsync<DomainException>(() => useCase.ExecuteAsync(taskItem.Id));

        repositoryMock.Verify(
            repository => repository.UpdateAsync(It.IsAny<TaskItem>()),
            Times.Never);
    }
}
