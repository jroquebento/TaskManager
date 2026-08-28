using Microsoft.AspNetCore.Mvc;
using TaskManager.Domain.Exceptions;

namespace TaskManager.IntegrationTests.TestControllers;

[ApiController]
[Route("test/exceptions")]
public class ExceptionTestController : ControllerBase
{
    [HttpGet]
    public IActionResult ThrowException()
    {
        throw new Exception("Erro interno de teste.");
    }

    [HttpGet("domain")]
    public IActionResult ThrowDomainException()
    {
        throw new DomainException("Erro de domínio de teste.");
    }
}