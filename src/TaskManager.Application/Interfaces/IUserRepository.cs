using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<bool> ExistsByEmailAsync(string email);
}
