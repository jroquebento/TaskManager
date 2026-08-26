using System.Net;
using System.Net.Http.Json;
using TaskManager.Application.DTOs;
using TaskManager.IntegrationTests.Fixtures;

namespace TaskManager.IntegrationTests.Controllers;

public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public AuthControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_ShouldReturnOk_WhenCredentialsAreValid()
    {
        await _factory.ResetDatabaseAsync();

        var email = $"{Guid.NewGuid()}@email.com";
        var password = "123456";

        var createUserRequest = new
        {
            name = "Usuário de login",
            email,
            password
        };

        var createUserResponse = await _client.PostAsJsonAsync(
        "/api/Users",
        createUserRequest);

        Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

        var loginRequest = new
        {
            email,
            password
        };

        var response = await _client.PostAsJsonAsync(
        "/api/Auth/login",
        loginRequest);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var loginResponse =
            await response.Content.ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(loginResponse);
        Assert.NotEmpty(loginResponse.Token);
    }


    [Fact]
    public async Task Login_ShouldReturnBadRequest_WhenUserDoesNotExist()
    {
        await _factory.ResetDatabaseAsync();

        var request = new
        {
            email = "inexistente@email.com",
            password = "123456"
        };

        var response = await _client.PostAsJsonAsync(
            "/api/Auth/login",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_ShouldReturnBadRequest_WhenPasswordIsInvalid()
    {
        await _factory.ResetDatabaseAsync();

        var email = $"{Guid.NewGuid()}@email.com";
        var password = "123456";

        var createUserRequest = new
        {
            name = "Usuário de login",
            email,
            password
        };

        var createUserResponse = await _client.PostAsJsonAsync(
            "/api/Users",
            createUserRequest);

        Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

        var loginRequest = new
        {
            email,
            password = "senha-incorreta"
        };

        var response = await _client.PostAsJsonAsync(
            "/api/Auth/login",
            loginRequest);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
