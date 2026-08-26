using Moq;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Application.UseCases.Login;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions;

namespace TaskManager.UnitTests.UseCases;

public class LoginUseCaseTests
{
    [Fact]
    public async Task Execute_ShouldReturnToken_WhenCredentialsAreValid()
    {
        var user = new User(
            Guid.NewGuid(),
            "João",
            "joao@email.com",
            "hashed-password");

        var request = new LoginRequest
        {
            Email = "joao@email.com",
            Password = "123456"
        };

        var repositoryMock = new Mock<IUserRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher>();
        var jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();

        repositoryMock
            .Setup(repository => repository.GetByEmailAsync(request.Email))
            .ReturnsAsync(user);

        passwordHasherMock
            .Setup(hasher => hasher.VerifyPassword(
                request.Password,
                user.PasswordHash))
            .Returns(true);

        jwtTokenGeneratorMock
            .Setup(generator => generator.Generate(user.Id))
            .Returns("jwt-token");

        var useCase = new LoginUseCase(
            repositoryMock.Object,
            passwordHasherMock.Object,
            jwtTokenGeneratorMock.Object);

        var result = await useCase.Execute(request);

        Assert.NotNull(result);
        Assert.Equal("jwt-token", result.Token);

        passwordHasherMock.Verify(
            hasher => hasher.VerifyPassword(
                request.Password,
                user.PasswordHash),
            Times.Once);

        jwtTokenGeneratorMock.Verify(
            generator => generator.Generate(user.Id),
            Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenUserDoesNotExist()
    {
        var request = new LoginRequest
        {
            Email = "inexistente@email.com",
            Password = "123456"
        };

        var repositoryMock = new Mock<IUserRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher>();
        var jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();

        repositoryMock
            .Setup(repository => repository.GetByEmailAsync(request.Email))
            .ReturnsAsync((User?)null);

        var useCase = new LoginUseCase(
            repositoryMock.Object,
            passwordHasherMock.Object,
            jwtTokenGeneratorMock.Object);

        await Assert.ThrowsAsync<DomainException>(
            () => useCase.Execute(request));

        passwordHasherMock.Verify(
            hasher => hasher.VerifyPassword(
                It.IsAny<string>(),
                It.IsAny<string>()),
            Times.Never);

        jwtTokenGeneratorMock.Verify(
            generator => generator.Generate(It.IsAny<Guid>()),
            Times.Never);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenPasswordIsInvalid()
    {
        var user = new User(
            Guid.NewGuid(),
            "João",
            "joao@email.com",
            "hashed-password");

        var request = new LoginRequest
        {
            Email = "joao@email.com",
            Password = "senha-incorreta"
        };

        var repositoryMock = new Mock<IUserRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher>();
        var jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();

        repositoryMock
            .Setup(repository => repository.GetByEmailAsync(request.Email))
            .ReturnsAsync(user);

        passwordHasherMock
            .Setup(hasher => hasher.VerifyPassword(
                request.Password,
                user.PasswordHash))
            .Returns(false);

        var useCase = new LoginUseCase(
            repositoryMock.Object,
            passwordHasherMock.Object,
            jwtTokenGeneratorMock.Object);

        await Assert.ThrowsAsync<DomainException>(
            () => useCase.Execute(request));

        jwtTokenGeneratorMock.Verify(
            generator => generator.Generate(It.IsAny<Guid>()),
            Times.Never);
    }
}
