using Moq;
using TaskManager.Application.Interfaces;
using TaskManager.Application.UseCases.GetTaskById;
using TaskManager.Domain.Entities;

namespace TaskManager.UnitTests.UseCases;

public class GetTaskByIdUseCaseTests
{
    private readonly Guid _userId = Guid.NewGuid();

    [Fact]
    public async Task ExecuteAsync_ShouldReturnTaskItemWhenTaskExists()
    {
        TaskItem taskItem = new (_userId, "Tarefa 1", null, null);

        var repositoryMock = new Mock<ITaskRepository>();

        repositoryMock
            .Setup(repository => repository.GetByIdAsync(taskItem.Id, _userId))
            .ReturnsAsync(taskItem);

        var currentUserMock = new Mock<ICurrentUser>();

        currentUserMock
            .Setup(currentUser => currentUser.UserId)
            .Returns(_userId);

        var useCase = new GetTaskByIdUseCase(
            repositoryMock.Object,
            currentUserMock.Object);

        var result = await useCase.ExecuteAsync(taskItem.Id);

        Assert.Equal(taskItem, result);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNullWhenTaskDoesNotExist()
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

        var useCase = new GetTaskByIdUseCase(
            repositoryMock.Object,
            currentUserMock.Object);

        var result = await useCase.ExecuteAsync(id);

        Assert.Null(result);
    }
}