using Moq;
using TaskManager.Application.Interfaces;
using TaskManager.Application.UseCases.ListTasks;
using TaskManager.Domain.Entities;

namespace TaskManager.UnitTests.UseCases;

public class ListTasksUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldReturnAlltTasks() 
    {
        // Arrange
        var tasks = new List<TaskItem>
        {
            new TaskItem("Tarefa 1", null, null),
            new TaskItem("Tarefa 2", null, null)
        };

        var repositoryMock = new Mock<ITaskRepository>();
        repositoryMock
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(tasks);

        var useCase = new ListTasksUseCase(repositoryMock.Object);


        // Act
        var result = await useCase.ExecuteAsync();

        //Assert
        Assert.Equal(tasks, result);
    }
}
