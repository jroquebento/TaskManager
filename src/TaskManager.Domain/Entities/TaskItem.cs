using TaskManager.Domain.Enums;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Domain.Entities;

public class TaskItem
{
    public Guid Id { get; private set; } 
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public TaskItemStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? DueDate { get; private set; }

    public TaskItem(string title, string? description, DateTime? dueDate)
    {
        if (string.IsNullOrWhiteSpace(title)) 
        {
            throw new DomainException("O título da tarefa é obrigatório.");
        }

        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        DueDate = dueDate;
        Status = TaskItemStatus.Pending;
        CreatedAt = DateTime.UtcNow;
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

    public void Update(string title, string? description, DateTime? dueDate) 
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainException("O título da tarefa é obrigatório.");
        }

        Title = title;
        Description = description;
        DueDate = dueDate;
    }
} 
