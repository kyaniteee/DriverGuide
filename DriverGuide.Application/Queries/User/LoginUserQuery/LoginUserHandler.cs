using DriverGuide.Application.Services;
using DriverGuide.Domain.Interfaces;
using MediatR;
using System.Security.Claims;

namespace DriverGuide.Application.Queries;

public class LoginUserHandler(
    IUserRepository userRepository,
    IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<LoginUserQuery, string>
{
    public async Task<string> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetWithRolesAndClaimsAsync(request.Login)
            ?? throw new UnauthorizedAccessException("Nieprawid這wy login lub has這");

        var passwordValid = await userRepository.VerifyPasswordAsync(user, request.Password);
        if (!passwordValid)
            throw new UnauthorizedAccessException("Nieprawid這wy login lub has這");

        var token = jwtTokenGenerator.GenerateToken(
            userId: user.Id.ToString(),
            userName: user.UserName!,
            email: user.Email!,
            roles: user.UserRoles?.Select(ur => ur.Role?.Name ?? "User").ToList() ?? new List<string> { "User" },
            additionalClaims: user.Claims?.Select(c => new Claim(c.ClaimType!, c.ClaimValue!)).ToList()
        );

        return token;
    }
}
