using System.Net;
using System.Net.Http.Json;
using TaskManager.Application.DTOs;
using TaskManager.IntegrationTests.Fixtures;

namespace TaskManager.IntegrationTests.Controllers;

public class UsersControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public UsersControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Create_ShouldReturnCreated_WhenDataIsValid()
    {
        await _factory.ResetDatabaseAsync();

        var request = new
        {
            name = "Usuário de integração",
            email = $"{Guid.NewGuid()}@email.com",
            password = "123456"
        };

        var response = await _client.PostAsJsonAsync(
            "/api/Users",
            request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var user = await response.Content.ReadFromJsonAsync<UserResponse>();

        Assert.NotNull(user);
        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal(request.name, user.Name);
        Assert.Equal(request.email, user.Email);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenEmailAlreadyExists()
    {
        await _factory.ResetDatabaseAsync();

        var email = $"{Guid.NewGuid()}@email.com";

        var request = new
        {
            name = "Usuário de integração",
            email,
            password = "123456"
        };

        var firstResponse = await _client.PostAsJsonAsync(
            "/api/Users",
            request);

        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);

        var secondResponse = await _client.PostAsJsonAsync(
            "/api/Users",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, secondResponse.StatusCode);
    }
}
