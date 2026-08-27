using System.Net.Mail;
using System.Text.RegularExpressions;
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

        ValidateEmail(email);

        if (string.IsNullOrWhiteSpace(passwordHash)) 
        {
            throw new DomainException("A senha do usuário é obrigatória");
        }

        Id = id; 
        Name = name; 
        Email = email;
        PasswordHash = passwordHash;
    }
    private static void ValidateEmail(string email)
    {
        if (!Regex.IsMatch(email, @"^[^@\s]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)+$"))
        {
            throw new DomainException("O e-mail do usuário é inválido.");
        }
    }
}
