using System.Net;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Enums;
using TaskManager.IntegrationTests.Fixtures;

namespace TaskManager.IntegrationTests.Controllers;

public class TasksControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public TasksControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ShouldReturnOnlyTasksFromAuthenticatedUser()
    {
        await _factory.ResetDatabaseAsync();

        var user1 = await CreateUserAsync();
        var user2 = await CreateUserAsync();

        await AuthenticateAsync(user1, "123456");

        var task1 = await CreateTaskAsyncForUser(user1);

        await AuthenticateAsync(user2, "123456");

        var task2 = await CreateTaskAsyncForUser(user2);

        await AuthenticateAsync(user1, "123456");

        var response = await _client.GetAsync("/api/Tasks");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var tasks = await response.Content.ReadFromJsonAsync<List<TaskResponse>>();

        Assert.NotNull(tasks);

        Assert.Contains(tasks, task => task.Id == task1.Id);
        Assert.DoesNotContain(tasks, task => task.Id == task2.Id);
    }

    [Fact]
    public async Task Create_ShouldCreateTask()
    {
        await _factory.ResetDatabaseAsync();

        var user = await CreateUserAsync();

        await AuthenticateAsync(user, "123456");

        var request = new
        {
            title = "Tarefa de integração",
            description = "Teste de integração",
            dueDate = DateTime.UtcNow.AddDays(1)
        };

        var response = await _client.PostAsJsonAsync("/api/Tasks", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdTask = await response.Content.ReadFromJsonAsync<TaskResponse>();

        Assert.NotNull(createdTask);
        Assert.Equal(request.title, createdTask.Title);
        Assert.Equal(request.description, createdTask.Description);

        var getResponse = await _client.GetAsync($"/api/Tasks/{createdTask.Id}");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var retrievedTask = await getResponse.Content.ReadFromJsonAsync<TaskResponse>();

        Assert.NotNull(retrievedTask);
        Assert.Equal(createdTask.Id, retrievedTask.Id);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenTaskDoesNotExist()
    {
        await _factory.ResetDatabaseAsync();

        var user = await CreateUserAsync();

        await AuthenticateAsync(user, "123456");

        var id = Guid.NewGuid();

        var response = await _client.GetAsync($"/api/Tasks/{id}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenTaskBelongsToAnotherUser()
    {
        await _factory.ResetDatabaseAsync();

        var user1 = await CreateUserAsync();
        var user2 = await CreateUserAsync();

        await AuthenticateAsync(user1, "123456");

        var task = await CreateTaskAsyncForUser(user1);

        await AuthenticateAsync(user2, "123456");

        var response = await _client.GetAsync(
            $"/api/Tasks/{task.Id}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Update_ShouldUpdateTask()
    {
        await _factory.ResetDatabaseAsync();

        var createdTask = await CreateTaskAsync();

        var updateRequest = new
        {
            title = "Tarefa atualizada",
            description = "Descrição atualizada",
            dueDate = DateTime.UtcNow.AddDays(5)
        };

        var updateResponse = await _client.PutAsJsonAsync(
            $"/api/Tasks/{createdTask.Id}",
            updateRequest);

        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        var updatedTask = await updateResponse.Content.ReadFromJsonAsync<TaskResponse>();

        Assert.NotNull(updatedTask);
        Assert.Equal(createdTask.Id, updatedTask.Id);
        Assert.Equal(updateRequest.title, updatedTask.Title);
        Assert.Equal(updateRequest.description, updatedTask.Description);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenTaskBelongsToAnotherUser()
    {
        await _factory.ResetDatabaseAsync();

        var user1 = await CreateUserAsync();
        var user2 = await CreateUserAsync();

        await AuthenticateAsync(user1, "123456");

        var task = await CreateTaskAsyncForUser(user1);

        await AuthenticateAsync(user2, "123456");

        var updateRequest = new
        {
            title = "Tentativa de alteração",
            description = "Usuário incorreto",
            dueDate = DateTime.UtcNow.AddDays(10)
        };

        var response = await _client.PutAsJsonAsync(
            $"/api/Tasks/{task.Id}",
            updateRequest);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldDeleteTask()
    {
        await _factory.ResetDatabaseAsync();

        var createdTask = await CreateTaskAsync();

        var deleteResponse = await _client.DeleteAsync($"/api/Tasks/{createdTask.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/Tasks/{createdTask.Id}");

        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenTaskBelongsToAnotherUser()
    {
        await _factory.ResetDatabaseAsync();

        var user1 = await CreateUserAsync();
        var user2 = await CreateUserAsync();

        await AuthenticateAsync(user1, "123456");

        var task = await CreateTaskAsyncForUser(user1);

        await AuthenticateAsync(user2, "123456");

        var response = await _client.DeleteAsync(
            $"/api/Tasks/{task.Id}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Start_ShouldStartTask()
    {
        await _factory.ResetDatabaseAsync();

        var createdTask = await CreateTaskAsync();

        var response = await _client.PostAsync(
            $"/api/Tasks/{createdTask.Id}/start",
            null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var getResponse = await _client.GetAsync(
            $"/api/Tasks/{createdTask.Id}");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var updatedTask = await getResponse.Content.ReadFromJsonAsync<TaskResponse>();

        Assert.NotNull(updatedTask);
        Assert.Equal(TaskItemStatus.InProgress, updatedTask.Status);
    }

    [Fact]
    public async Task Start_ShouldReturnNotFound_WhenTaskBelongsToAnotherUser()
    {
        await _factory.ResetDatabaseAsync();

        var user1 = await CreateUserAsync();
        var user2 = await CreateUserAsync();

        await AuthenticateAsync(user1, "123456");

        var task = await CreateTaskAsyncForUser(user1);

        await AuthenticateAsync(user2, "123456");

        var response = await _client.PostAsync(
            $"/api/Tasks/{task.Id}/start",
            null);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Complete_ShouldCompleteTask()
    {
        await _factory.ResetDatabaseAsync();

        var createdTask = await CreateTaskAsync();

        var startResponse = await _client.PostAsync(
            $"/api/Tasks/{createdTask.Id}/start",
            null);

        Assert.Equal(HttpStatusCode.OK, startResponse.StatusCode);

        var completeResponse = await _client.PostAsync(
            $"/api/Tasks/{createdTask.Id}/complete",
            null);

        Assert.Equal(HttpStatusCode.OK, completeResponse.StatusCode);

        var getResponse = await _client.GetAsync(
            $"/api/Tasks/{createdTask.Id}");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var completedTask = await getResponse.Content.ReadFromJsonAsync<TaskResponse>();

        Assert.NotNull(completedTask);
        Assert.Equal(TaskItemStatus.Completed, completedTask.Status);
    }

    [Fact]
    public async Task Complete_ShouldReturnNotFound_WhenTaskBelongsToAnotherUser()
    {
        await _factory.ResetDatabaseAsync();

        var user1 = await CreateUserAsync();
        var user2 = await CreateUserAsync();

        await AuthenticateAsync(user1, "123456");

        var task = await CreateTaskAsyncForUser(user1);

        await AuthenticateAsync(user2, "123456");

        var response = await _client.PostAsync(
            $"/api/Tasks/{task.Id}/complete",
            null);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Start_ShouldReturnBadRequest_WhenTaskIsAlreadyInProgress()
    {
        await _factory.ResetDatabaseAsync();

        var createdTask = await CreateTaskAsync();

        var firstStartResponse = await _client.PostAsync(
            $"/api/Tasks/{createdTask.Id}/start",
            null);

        Assert.Equal(HttpStatusCode.OK, firstStartResponse.StatusCode);

        var secondStartResponse = await _client.PostAsync(
            $"/api/Tasks/{createdTask.Id}/start",
            null);

        Assert.Equal(HttpStatusCode.BadRequest, secondStartResponse.StatusCode);
    }

    [Fact]
    public async Task Complete_ShouldReturnBadRequest_WhenTaskIsPending()
    {
        await _factory.ResetDatabaseAsync();

        var createdTask = await CreateTaskAsync();

        var response = await _client.PostAsync(
            $"/api/Tasks/{createdTask.Id}/complete",
            null);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    private async Task<TaskResponse> CreateTaskAsync()
    {
        var user = await CreateUserAsync();

        await AuthenticateAsync(user, "123456");

        var request = new
        {
            userId = user.Id,
            title = "Tarefa de integração",
            description = "Teste de integração",
            dueDate = DateTime.UtcNow.AddDays(1)
        };

        var response = await _client.PostAsJsonAsync("/api/Tasks", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdTask = await response.Content.ReadFromJsonAsync<TaskResponse>();

        Assert.NotNull(createdTask);

        return createdTask;
    }

    private async Task<TaskResponse> CreateTaskAsyncForUser(UserResponse user)
    {
        await AuthenticateAsync(user, "123456");

        var request = new
        {
            title = "Tarefa de integração",
            description = "Teste de integração",
            dueDate = DateTime.UtcNow.AddDays(1)
        };

        var response = await _client.PostAsJsonAsync("/api/Tasks", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdTask = await response.Content.ReadFromJsonAsync<TaskResponse>();

        Assert.NotNull(createdTask);

        return createdTask;
    }


    private async Task<UserResponse> CreateUserAsync()
    {
        var request = new
        {
            name = "Usuário de teste",
            email = $"{Guid.NewGuid()}@email.com",
            password = "123456"
        };

        var response = await _client.PostAsJsonAsync("/api/Users", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdUser = await response.Content.ReadFromJsonAsync<UserResponse>();

        Assert.NotNull(createdUser);

        return createdUser;
    }

    private async Task AuthenticateAsync(UserResponse user, string password)
    {
        var loginRequest = new
        {
            email = user.Email,
            password
        };

        var response = await _client.PostAsJsonAsync(
            "/api/Auth/login",
            loginRequest);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var loginResponse =
            await response.Content.ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(loginResponse);

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                loginResponse.Token);
    }
}