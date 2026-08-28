using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;

        if (exception is DomainException)
        {
            context.Result = new BadRequestObjectResult(exception.Message);
            return;
        }

        context.Result = new ObjectResult("Ocorreu um erro interno no servidor.")
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}
