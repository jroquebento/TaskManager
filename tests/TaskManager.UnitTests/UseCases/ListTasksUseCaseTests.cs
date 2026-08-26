using Moq;
using TaskManager.Application.Interfaces;
using TaskManager.Application.UseCases.ListTasks;
using TaskManager.Domain.Entities;

namespace TaskManager.UnitTests.UseCases;

public class ListTasksUseCaseTests
{
    private readonly Guid _userId = Guid.NewGuid();

    [Fact]
    public async Task ExecuteAsync_ShouldReturnTasksForCurrentUser()
    {
     
        var tasks = new List<TaskItem>
        {
            new TaskItem(_userId, "Tarefa 1", null, null),
            new TaskItem(_userId, "Tarefa 2", null, null)
        };

        var repositoryMock = new Mock<ITaskRepository>();
        var currentUserMock = new Mock<ICurrentUser>();

        currentUserMock
            .Setup(currentUser => currentUser.UserId)
            .Returns(_userId);

        repositoryMock
            .Setup(repository => repository.GetAllByUserIdAsync(_userId))
            .ReturnsAsync(tasks);

        var useCase = new ListTasksUseCase(
            repositoryMock.Object,
            currentUserMock.Object);

        var result = await useCase.ExecuteAsync();

        Assert.Equal(tasks, result);

        repositoryMock.Verify(
            repository => repository.GetAllByUserIdAsync(_userId),
            Times.Once);
    }
}
