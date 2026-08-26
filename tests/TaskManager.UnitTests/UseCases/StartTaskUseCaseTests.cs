using Moq;
using TaskManager.Application.Interfaces;
using TaskManager.Application.UseCases.StartTask;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Exceptions;

namespace TaskManager.UnitTests.UseCases;

public class StartTaskUseCaseTests
{
    private readonly Guid _userId = Guid.NewGuid();
    [Fact]
    public async Task ExecuteAsync_ShouldStartTaskWhenTaskExists()
    {
        TaskItem taskItem = new(_userId, "Tarefa 1", null, null);

        var repositoryMock = new Mock<ITaskRepository>();

        repositoryMock
            .Setup(repository => repository.GetByIdAsync(taskItem.Id, _userId))
            .ReturnsAsync(taskItem);

        var currentUserMock = new Mock<ICurrentUser>();

        currentUserMock
            .Setup(user => user.UserId)
            .Returns(_userId);

        var useCase = new StartTaskUseCase(
            repositoryMock.Object,
            currentUserMock.Object);

        var result = await useCase.ExecuteAsync(taskItem.Id);

        Assert.True(result);
        Assert.Equal(TaskItemStatus.InProgress, taskItem.Status);

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
            .Setup(repository => repository.GetByIdAsync(id, _userId))
            .ReturnsAsync((TaskItem?)null);

        var currentUserMock = new Mock<ICurrentUser>();

        currentUserMock
            .Setup(user => user.UserId)
            .Returns(_userId);

        var useCase = new StartTaskUseCase(
            repositoryMock.Object,
            currentUserMock.Object);

        var result = await useCase.ExecuteAsync(id);

        Assert.False(result);

        repositoryMock.Verify(
            repository => repository.UpdateAsync(It.IsAny<TaskItem>()),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowExceptionWhenTaskIsAlreadyInProgress()
    {
        TaskItem taskItem = new(_userId, "Tarefa 1", null, null);
        taskItem.Start();

        var repositoryMock = new Mock<ITaskRepository>();

        repositoryMock
            .Setup(repository => repository.GetByIdAsync(taskItem.Id, _userId))
            .ReturnsAsync(taskItem);

        var currentUserMock = new Mock<ICurrentUser>();

        currentUserMock
            .Setup(user => user.UserId)
            .Returns(_userId);

        var useCase = new StartTaskUseCase(
            repositoryMock.Object,
            currentUserMock.Object);

        await Assert.ThrowsAsync<DomainException>(() => useCase.ExecuteAsync(taskItem.Id));

        repositoryMock.Verify(
            repository => repository.UpdateAsync(It.IsAny<TaskItem>()),
            Times.Never);
    }
}
