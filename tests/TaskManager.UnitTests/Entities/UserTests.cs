using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions;

namespace TaskManager.UnitTests.Entities;

public class UserTests
{
    [Fact]
    public void Constructor_ShouldCreateUser_WhenDataIsValid()
    {
        var id = Guid.NewGuid();

        var user = new User(
            id,
            "João",
            "joao@email.com",
            "passwordHash");

        Assert.Equal(id, user.Id);
        Assert.Equal("João", user.Name);
        Assert.Equal("joao@email.com", user.Email);
        Assert.Equal("passwordHash", user.PasswordHash);
        Assert.Empty(user.TaskItems);
    }

    [Fact]
    public void Constructor_ShouldThrowExceptionWhenIdIsEmpty()
    {
        Assert.Throws<DomainException>(() =>
            new User(
                Guid.Empty,
                "João",
                "joao@email.com",
                "passwordHash"));
    }

    [Fact]
    public void Constructor_ShouldThrowExceptionWhenNameIsEmpty()
    {
        Assert.Throws<DomainException>(() =>
            new User(
                Guid.NewGuid(),
                "",
                "joao@email.com",
                "passwordHash"));
    }

    [Fact]
    public void Constructor_ShouldThrowExceptionWhenEmailIsEmpty()
    {
        Assert.Throws<DomainException>(() =>
            new User(
                Guid.NewGuid(),
                "João",
                "",
                "passwordHash"));
    }

    [Fact]
    public void Constructor_ShouldThrowExceptionWhenPasswordHashIsEmpty()
    {
        Assert.Throws<DomainException>(() =>
            new User(
                Guid.NewGuid(),
                "João",
                "joao@email.com",
                ""));
    }

    [Fact]
    public void Constructor_ShouldThrowExceptionWhenEmailIsInvalid()
    {
        Assert.Throws<DomainException>(() =>
           new User(
               Guid.NewGuid(),
               "João",
               "email-invalido",
               "passwordHash"));
    }

    [Fact]
    public void Constructor_ShouldThrowExceptionWhenEmailHasNoDotInDomain()
    {
        Assert.Throws<DomainException>(() =>
            new User(
                Guid.NewGuid(),
                "João",
                "teste@a",
                "passwordHash"));
    }
}
