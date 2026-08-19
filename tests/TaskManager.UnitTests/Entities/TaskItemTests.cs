using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Exceptions;

namespace TaskManager.UnitTests.Entities;

public class TaskItemTests
{
    [Fact]
    public void Start_ShouldChangeStatusToInProgress() 
    {
        // Arrange
        var task = new TaskItem("Tarefa de teste",null,null);


        // Act
        task.Start();

        // Assert
        Assert.Equal(TaskItemStatus.InProgress, task.Status);
    }
    [Fact]
    
    public void Complete_ShouldChangeStatusToCompleted()
    {
        var task = new TaskItem("Tarefa de teste", null, null);
        task.Start();

        task.Complete();

        Assert.Equal(TaskItemStatus.Completed, task.Status);
    }
    [Fact]
    public void Complete_ShouldThrowExceptionWhenTaskIsPending()
    {
        var task = new TaskItem("Tarefa de teste", null, null);
     
        Assert.Throws<DomainException>(() => task.Complete());
    }

    [Fact]
    public void Start_ShouldThrowExceptionWhenTaskIsInProgress() 
    {
        var task = new TaskItem("Tarefa de teste", null, null);
        task.Start();

        Assert.Throws<DomainException>(() => task.Start());
    }

    [Fact]
    public void Constructor_ShouldThrowExceptionWhenTitleIsNull() 
    {
        Assert.Throws<DomainException>(() => new TaskItem(null!, null, null));
    }

    [Fact]
    public void Constructor_ShouldThrowExceptionWhenTitleIsEmpty()
    {
        Assert.Throws<DomainException>(() => new TaskItem("", null, null));
    }

    [Fact]
    public void Constructor_ShouldThrowExceptionWhenTitleIsWhiteSpace() 
    {
        Assert.Throws<DomainException>(() => new TaskItem("   ", null, null));
    }
}
