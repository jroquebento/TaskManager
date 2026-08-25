using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Exceptions;

namespace TaskManager.UnitTests.Entities;

public class TaskItemTests
{
    private readonly Guid _userId = Guid.NewGuid();

    [Fact]
    public void Start_ShouldChangeStatusToInProgress() 
    {
        // Arrange
        var task = new TaskItem(_userId, "Tarefa de teste",null,null);


        // Act
        task.Start();

        // Assert
        Assert.Equal(TaskItemStatus.InProgress, task.Status);
    }
    [Fact]
    
    public void Complete_ShouldChangeStatusToCompleted()
    {
        var task = new TaskItem(_userId,"Tarefa de teste", null, null);
        task.Start();

        task.Complete();

        Assert.Equal(TaskItemStatus.Completed, task.Status);
    }
    [Fact]
    public void Complete_ShouldThrowExceptionWhenTaskIsPending()
    {
        var task = new TaskItem(_userId, "Tarefa de teste", null, null);
     
        Assert.Throws<DomainException>(() => task.Complete());
    }

    [Fact]
    public void Start_ShouldThrowExceptionWhenTaskIsInProgress() 
    {
        var task = new TaskItem(_userId, "Tarefa de teste", null, null);
        task.Start();

        Assert.Throws<DomainException>(() => task.Start());
    }

    [Fact]
    public void Constructor_ShouldThrowExceptionWhenTitleIsNull() 
    {
        Assert.Throws<DomainException>(() => new TaskItem(_userId, null!, null, null));
    }

    [Fact]
    public void Constructor_ShouldThrowExceptionWhenTitleIsEmpty()
    {
        Assert.Throws<DomainException>(() => new TaskItem(_userId, "", null, null));
    }

    [Fact]
    public void Constructor_ShouldThrowExceptionWhenTitleIsWhiteSpace() 
    {
        Assert.Throws<DomainException>(() => new TaskItem(_userId, "   ", null, null));
    }

    [Fact]
    public void Update_ShouldChangeTaskData()
    {
        TaskItem taskItem = new(_userId, "Tarefa original",null,null);

        taskItem.Update("Tarefa atualizada", "Nova descrição", new DateTime(2026,05,01));

        Assert.Equal("Tarefa atualizada", taskItem.Title);
        Assert.Equal("Nova descrição", taskItem.Description);
        Assert.Equal(new DateTime(2026, 05, 01), taskItem.DueDate);
    }

    [Fact]
    public void Update_ShouldThrowExceptionWhenTitleIsInvalid() 
    {
        TaskItem taskItem = new(_userId, "Tarefa original", null, null);

        Assert.Throws<DomainException>(() => 
        taskItem.Update("", "Nova descrição", new DateTime(2026, 05, 01)));
    }
}
