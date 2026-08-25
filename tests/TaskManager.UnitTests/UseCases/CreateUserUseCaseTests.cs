using Moq;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Application.UseCases.CreateUser;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions;

namespace TaskManager.UnitTests.UseCases;

public class CreateUserUseCaseTests
{
    [Fact]
    public async Task Execute_ShouldCreateUser_WhenDataIsValid()
    {
        var request = new CreateUserRequest
        {
            Name = "João",
            Email = "joao@email.com",
            Password = "123456"
        };

        var repositoryMock = new Mock<IUserRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher>();

        passwordHasherMock
            .Setup(hasher => hasher.HashPassword(request.Password))
            .Returns("hashed-password");

        var useCase = new CreateUserUseCase(repositoryMock.Object, passwordHasherMock.Object);

        var result = await useCase.Execute(request);

        Assert.NotNull(result);
        Assert.Equal(request.Name, result.Name);
        Assert.Equal(request.Email, result.Email);
        Assert.Equal("hashed-password", result.PasswordHash);

        passwordHasherMock.Verify(
            hasher => hasher.HashPassword(request.Password),
            Times.Once);

        repositoryMock.Verify(repository => repository.AddAsync(It.Is<User>(
            user => user.Name == request.Name &&
            user.Email == request.Email &&
            user.PasswordHash == "hashed-password")),
            Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenEmailAlreadyExists() 
    {
        var request = new CreateUserRequest { 
            Name = "João", 
            Email = "joao@email.com", 
            Password = "123456" 
        };

        var repositoryMock = new Mock<IUserRepository>(); 
        var passwordHasherMock = new Mock<IPasswordHasher>();

        repositoryMock
            .Setup(repository => repository
            .ExistsByEmailAsync(request.Email))
            .ReturnsAsync(true);

        var useCase = new CreateUserUseCase(repositoryMock.Object, passwordHasherMock.Object);

        await Assert.ThrowsAsync<DomainException>(() => useCase.Execute(request));

        passwordHasherMock.Verify(
            hasher => hasher.HashPassword(It.IsAny<string>()), 
            Times.Never);

        repositoryMock.Verify(
            repository => repository.AddAsync(It.IsAny<User>()), 
            Times.Never);
    }
}
