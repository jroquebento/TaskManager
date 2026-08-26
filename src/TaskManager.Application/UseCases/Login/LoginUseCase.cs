using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Application.UseCases.Login;

public class LoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginResponse> Execute(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is null)
        {
            throw new DomainException("E-mail ou senha inválidos.");
        }

        var passwordIsValid = _passwordHasher.VerifyPassword(
            request.Password,
            user.PasswordHash);

        if (!passwordIsValid)
        {
            throw new DomainException("E-mail ou senha inválidos.");
        }

        var token = _jwtTokenGenerator.Generate(user.Id);

        return new LoginResponse
        {
            Token = token
        };
    }
}
