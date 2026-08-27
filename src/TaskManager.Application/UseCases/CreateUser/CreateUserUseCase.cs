using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Application.UseCases.CreateUser;

public class CreateUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<User> Execute(CreateUserRequest request) 
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email)) 
        {
            throw new DomainException("O e-mail informado já está cadastrado.");
        }

        if (string.IsNullOrWhiteSpace(request.Password)) 
        {
            throw new DomainException("A senha do usuário é obrigatória.");
        }

        var passwordHash = _passwordHasher.HashPassword(request.Password);

        var user = new User(Guid.NewGuid(), request.Name, request.Email, passwordHash);

        await _userRepository.AddAsync(user);

        return user;
    }
}


