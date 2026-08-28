using System.Net;
using TaskManager.IntegrationTests.Fixtures;

namespace TaskManager.IntegrationTests.Controllers;

public class ExceptionControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ExceptionControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ShouldReturnInternalServerError_WhenUnexpectedExceptionOccurs()
    {
        var response = await _client.GetAsync("/test/exceptions");

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnBadRequest_WhenDomainExceptionOccurs()
    {
        var response = await _client.GetAsync("/test/exceptions/domain");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var message = await response.Content.ReadAsStringAsync();

        Assert.Equal("Erro de domínio de teste.", message.Trim('"'));
    }
}