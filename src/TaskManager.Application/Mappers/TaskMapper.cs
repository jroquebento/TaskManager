using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Mappers;

public class TaskMapper
{
    public static TaskResponse ToResponse(TaskItem taskItem) 
    {
        return new TaskResponse
        {
            Id = taskItem.Id,
            Title = taskItem.Title,
            Description = taskItem.Description,
            Status = taskItem.Status,
            CreatedAt = taskItem.CreatedAt,
            DueDate = taskItem.DueDate
        };
    }    
}
