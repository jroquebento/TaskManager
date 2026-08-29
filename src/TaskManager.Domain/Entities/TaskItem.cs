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

    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    private TaskItem() { }

    public TaskItem(Guid userId, string title, string? description, DateTime? dueDate)
    {
        if (userId == Guid.Empty)
        {
            throw new DomainException("O usuário da tarefa é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainException("O título da tarefa é obrigatório.");
        }

        Id = Guid.NewGuid();
        UserId = userId;
        Title = title;
        Description = description;
        Status = TaskItemStatus.Pending;
        CreatedAt = DateTime.UtcNow;

        ValidateDueDate(dueDate);

        DueDate = dueDate;
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

        ValidateDueDate(dueDate);

        Title = title;
        Description = description;
        DueDate = dueDate;
    }

    private void ValidateDueDate(DateTime? dueDate)
    {
        if (dueDate.HasValue && dueDate.Value < CreatedAt)
        {
            throw new DomainException("A data de vencimento não pode ser anterior à data de criação.");
        }
    }
}
