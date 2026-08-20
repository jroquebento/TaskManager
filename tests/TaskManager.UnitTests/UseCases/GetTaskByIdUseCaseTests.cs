using Castle.Core.Logging;
using Moq;
using TaskManager.Application.Interfaces;
using TaskManager.Application.UseCases.GetTaskById;
using TaskManager.Domain.Entities;

namespace TaskManager.UnitTests.UseCases;

public class GetTaskByIdUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldReturnTaskItemWhenTaskExists() 
    {
        TaskItem taskItem = new TaskItem("Tarefa 1", null, null);

        var repositoryMock = new Mock<ITaskRepository>();
        repositoryMock
            .Setup(repository => repository.GetByIdAsync(taskItem.Id))
            .ReturnsAsync(taskItem);

        var useCase = new GetTaskByIdUseCase(repositoryMock.Object);

        var result = await useCase.ExecuteAsync(taskItem.Id);

        Assert.Equal(taskItem, result);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNullWhenTaskDoesNotExist()
    {
        var id = Guid.NewGuid();
        
        var repositoryMock = new Mock<ITaskRepository>();
        repositoryMock
            .Setup(repository => repository.GetByIdAsync(id))
            .ReturnsAsync((TaskItem?)null);

        var useCase = new GetTaskByIdUseCase(repositoryMock.Object);

        var result = await useCase.ExecuteAsync(id);

        Assert.Null(result);
    }
}
