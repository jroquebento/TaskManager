namespace TaskManager.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string Generate(Guid userId);
}
