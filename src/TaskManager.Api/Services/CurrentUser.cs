using System.Security.Claims;
using TaskManager.Application.Interfaces;

namespace TaskManager.Api.Services;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId 
    {
        get 
        {
            var userId = _httpContextAccessor.HttpContext?
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId)) 
            {
                throw new InvalidOperationException(
                    "Usuário autenticado não encontrado.");
            }
            return Guid.Parse(userId);
        }
    
    }
}
