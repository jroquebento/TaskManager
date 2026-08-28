using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using TaskManager.Infrastructure.Security;

namespace TaskManager.UnitTests.Security;

public class JwtTokenGeneratorTests
{
    [Fact]
    public void Generate_ShouldReturnToken_WhenConfigurationIsValid()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:SecretKey"] = "uma-chave-secreta-de-teste-com-pelo-menos-32-caracteres",
                ["Jwt:Issuer"] = "TaskManager",
                ["Jwt:Audience"] = "TaskManagerClient",
                ["Jwt:ExpirationInMinutes"] = "60"
            })
            .Build();

        var generator = new JwtTokenGenerator(configuration);

        var userId = Guid.NewGuid();

        var token = generator.Generate(userId);

        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

    [Fact]
    public void Generate_ShouldContainUserIdClaim()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:SecretKey"] = "uma-chave-secreta-de-teste-com-pelo-menos-32-caracteres",
                ["Jwt:Issuer"] = "TaskManager",
                ["Jwt:Audience"] = "TaskManagerClient",
                ["Jwt:ExpirationInMinutes"] = "60"
            })
            .Build();

        var generator = new JwtTokenGenerator(configuration);

        var userId = Guid.NewGuid();

        var token = generator.Generate(userId);

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        var claim = jwt.Claims.FirstOrDefault(
            claim => claim.Type == ClaimTypes.NameIdentifier);

        Assert.NotNull(claim);
        Assert.Equal(userId.ToString(), claim.Value);
    }

    [Fact]
    public void Generate_ShouldUseConfiguredIssuerAudienceAndExpiration()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:SecretKey"] = "uma-chave-secreta-de-teste-com-pelo-menos-32-caracteres",
                ["Jwt:Issuer"] = "TaskManager",
                ["Jwt:Audience"] = "TaskManagerClient",
                ["Jwt:ExpirationInMinutes"] = "60"
            })
            .Build();

        var generator = new JwtTokenGenerator(configuration);

        var token = generator.Generate(Guid.NewGuid());

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        Assert.Equal("TaskManager", jwt.Issuer);
        Assert.Contains("TaskManagerClient", jwt.Audiences);

        Assert.True(jwt.ValidTo > DateTime.UtcNow);
        Assert.True(jwt.ValidTo <= DateTime.UtcNow.AddMinutes(61));
    }

    [Fact]
    public void Generate_ShouldThrowException_WhenSecretKeyIsMissing()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Issuer"] = "TaskManager",
                ["Jwt:Audience"] = "TaskManagerClient",
                ["Jwt:ExpirationInMinutes"] = "60"
            })
            .Build();

        var generator = new JwtTokenGenerator(configuration);

        var exception = Assert.Throws<InvalidOperationException>(
            () => generator.Generate(Guid.NewGuid()));

        Assert.Equal(
            "A chave secreta do JWT não foi configurada.",
            exception.Message);
    }
}
