using TaskManager.Domain.Exceptions;

namespace TaskManager.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;

    public ICollection<TaskItem> TaskItems { get; private set; } = [];

    public User(Guid id , string name, string email, string passwordHash)
    {
        if (id == Guid.Empty) 
        {
            throw new DomainException("O ID do usuário é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(name)) 
        {
            throw new DomainException("O nome do usuário é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(email)) 
        {
            throw new DomainException("O e-mail do usuário é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(passwordHash)) 
        {
            throw new DomainException("A senha do usuário é obrigatória");
        }

        Id = id; 
        Name = name; 
        Email = email;
        PasswordHash = passwordHash;
    }
}
