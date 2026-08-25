using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TaskManagerDbContext _context;

    public UserRepository(TaskManagerDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users.AnyAsync(user => user.Email == email);
    }
}
