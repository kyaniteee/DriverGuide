using DriverGuide.Application.Services;
using DriverGuide.Domain.Interfaces;
using MediatR;
using System.Security.Claims;

namespace DriverGuide.Application.Requests;

/// <summary>
/// Handler odpowiedzialny za obsługę procesu logowania użytkownika.
/// Weryfikuje dane uwierzytelniające i generuje token JWT dla zalogowanego użytkownika.
/// </summary>
/// <param name="userRepository">Repozytorium użytkowników do pobierania danych i weryfikacji hasła.</param>
/// <param name="jwtTokenGenerator">Generator tokenów JWT używany do tworzenia tokenu dostępu.</param>
public class LoginUserHandler(
    IUserRepository userRepository,
    IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<LoginUserRequest, string>
{
    /// <summary>
    /// Obsługuje żądanie logowania użytkownika.
    /// Weryfikuje dane logowania, sprawdza hasło i generuje token JWT zawierający role i roszczenia użytkownika.
    /// </summary>
    /// <param name="request">Obiekt żądania zawierający login i hasło użytkownika.</param>
    /// <param name="cancellationToken">Token anulowania operacji asynchronicznej.</param>
    /// <returns>
    /// Token JWT jako string, który może być używany do uwierzytelniania kolejnych żądań.
    /// </returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Rzucany gdy użytkownik nie zostanie znaleziony lub hasło jest nieprawidłowe.
    /// </exception>
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