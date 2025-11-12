using System.Security.Claims;

namespace DriverGuide.Application.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(
        string userId,
        string userName,
        string email,
        List<string> roles,
        List<Claim>? additionalClaims = null);
}
