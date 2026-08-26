using TaskManager.Infrastructure.Security;

namespace TaskManager.UnitTests.Security;

public class Argon2PasswordHasherTests
{
    [Fact]
    public void VerifyPassword_ShouldReturnTrue_WhenPasswordIsCorrect() 
    {
        var hasher = new Argon2PasswordHasher();
        var password = "123456";

        var passwordHash = hasher.HashPassword(password);

        var result = hasher.VerifyPassword(password, passwordHash);

        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalse_WhenPasswordIsIncorrect()
    {
        var hasher = new Argon2PasswordHasher();
        var passwordHash = hasher.HashPassword("123456");

        var result = hasher.VerifyPassword("senha-incorreta", passwordHash);

        Assert.False(result);
    }

    [Fact]
    public void HashPassword_ShouldGenerateDifferentHashes_ForSamePassword()
    {
        var hasher = new Argon2PasswordHasher();

        var firstHash = hasher.HashPassword("123456");
        var secondHash = hasher.HashPassword("123456");

        Assert.NotEqual(firstHash, secondHash);
    }
}
