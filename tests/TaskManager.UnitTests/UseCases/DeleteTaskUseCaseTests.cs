using Moq;
using TaskManager.Application.Interfaces;
using TaskManager.Application.UseCases.DeleteTask;
using TaskManager.Domain.Entities;

namespace TaskManager.UnitTests.UseCases;

public class DeleteTaskUseCaseTests
{
    private readonly Guid _userId = Guid.NewGuid();

    [Fact]
    public async Task ExecuteAsync_ShouldDeleteTaskWhenTaskExists()
    {
        TaskItem taskItem = new(_userId, "Tarefa 1", null, null);

        var repositoryMock = new Mock<ITaskRepository>();

        repositoryMock
            .Setup(repository => repository.GetByIdAsync(taskItem.Id, _userId))
            .ReturnsAsync(taskItem);

        var currentUserMock = new Mock<ICurrentUser>();

        currentUserMock
            .Setup(currentUser => currentUser.UserId)
            .Returns(_userId);

        var useCase = new DeleteTaskUseCase(
            repositoryMock.Object,
            currentUserMock.Object);

        var result = await useCase.ExecuteAsync(taskItem.Id);

        Assert.True(result);

        repositoryMock.Verify(
            repository => repository.DeleteAsync(taskItem),
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
            .Setup(currentUser => currentUser.UserId)
            .Returns(_userId);

        var useCase = new DeleteTaskUseCase(
            repositoryMock.Object,
            currentUserMock.Object);

        var result = await useCase.ExecuteAsync(id);

        Assert.False(result);

        repositoryMock.Verify(
            repository => repository.DeleteAsync(It.IsAny<TaskItem>()),
            Times.Never);
    }
}