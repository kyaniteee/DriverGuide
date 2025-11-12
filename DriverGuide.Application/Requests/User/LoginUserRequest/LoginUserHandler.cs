using DriverGuide.Application.Services;
using DriverGuide.Domain.Interfaces;
using MediatR;
using System.Security.Claims;

namespace DriverGuide.Application.Requests;

public class LoginUserHandler(
    IUserRepository userRepository,
    IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<LoginUserRequest, string>
{
    public async Task<string> Handle(LoginUserRequest request, CancellationToken cancellationToken)
    {
        // Znajdź użytkownika po loginie lub e-mailu z rolami i roszczeniami
        var user = await userRepository.GetWithRolesAndClaimsAsync(request.Login)
            ?? throw new UnauthorizedAccessException("Nieprawidłowy login lub hasło");

        // Sprawdź hasło
        var passwordValid = await userRepository.VerifyPasswordAsync(user, request.Password);
        if (!passwordValid)
            throw new UnauthorizedAccessException("Nieprawidłowy login lub hasło");

        // Wygeneruj token JWT z roszczeniami użytkownika
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