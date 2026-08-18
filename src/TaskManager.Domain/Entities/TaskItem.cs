using TaskManager.Domain.Enums;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Domain.Entities;

public class TaskItem
{
    public Guid Id { get; set; } 
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public TaskItemStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }

    public TaskItem()
    {
        Status = TaskItemStatus.Pending;
    }

    public void Start() 
    {
        if (Status != TaskItemStatus.Pending)
        {
            throw new DomainException("A tarefa não pode ser iniciada neste estado.");
        }
        Status = TaskItemStatus.InProgress;
    }

    public void Complete()
    {
        if (Status != TaskItemStatus.InProgress) 
        {
            throw new DomainException("A tarefa não pode ser finalizada neste estado.");
        }
        Status = TaskItemStatus.Completed;
    }
} 
