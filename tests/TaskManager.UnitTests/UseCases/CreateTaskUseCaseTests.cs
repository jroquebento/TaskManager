using Moq;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Application.UseCases.CreateTask;
using TaskManager.Domain.Entities;

namespace TaskManager.UnitTests.UseCases;

public class CreateTaskUseCaseTests
{
    [Fact]
    public async Task Execute_ShouldCreateTaskForCurrentUser_WhenDataIsValid()
    {
        var userId = Guid.NewGuid();

        var request = new CreateTaskRequest
        {
            Title = "Tarefa de teste",
            Description = "Descrição da tarefa",
            DueDate = DateTime.UtcNow.AddDays(1)
        };

        var repositoryMock = new Mock<ITaskRepository>();
        var currentUserMock = new Mock<ICurrentUser>();

        currentUserMock
           .Setup(currentUser => currentUser.UserId)
           .Returns(userId);


        var useCase = new CreateTaskUseCase(
           repositoryMock.Object,
           currentUserMock.Object);

        var result = await useCase.Execute(request);

        Assert.NotNull(result);
        Assert.Equal(userId, result.UserId);
        Assert.Equal(request.Title, result.Title);
        Assert.Equal(request.Description, result.Description);
        Assert.Equal(request.DueDate, result.DueDate);

        repositoryMock.Verify(
           repository => repository.AddAsync(It.Is<TaskItem>(
               task => task.UserId == userId &&
                       task.Title == request.Title &&
                       task.Description == request.Description &&
                       task.DueDate == request.DueDate)),
           Times.Once);
    }
}
