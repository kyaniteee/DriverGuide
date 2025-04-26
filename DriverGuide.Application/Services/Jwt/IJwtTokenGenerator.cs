namespace DriverGuide.Application.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(string userName);
}
