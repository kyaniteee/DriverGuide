using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DriverGuide.Application.Requests;

/// <summary>
/// Handler odpowiedzialny za tworzenie nowego użytkownika w systemie.
/// Hashuje hasło, tworzy domyślną rolę i zapisuje dane użytkownika wraz z claims.
/// </summary>
/// <param name="userRepository">Repozytorium użytkowników do zapisu danych.</param>
public class CreateUserHandler(IUserRepository userRepository) : IRequestHandler<CreateUserRequest, Guid>
{
    /// <summary>
    /// Obsługuje żądanie utworzenia nowego użytkownika.
    /// Tworzy obiekt użytkownika, hashuje hasło, przypisuje rolę "User" 
    /// oraz dodaje claims zawierające imię, nazwisko i datę urodzenia.
    /// </summary>
    /// <param name="request">Obiekt żądania zawierający dane nowego użytkownika.</param>
    /// <param name="cancellationToken">Token anulowania operacji asynchronicznej.</param>
    /// <returns>
    /// GUID nowo utworzonego użytkownika.
    /// </returns>
    /// <remarks>
    /// Hasło jest hashowane przy użyciu PasswordHasher z ASP.NET Core Identity.
    /// Użytkownik domyślnie otrzymuje rolę "User".
    /// Dane osobowe (imię, nazwisko, data urodzenia) są przechowywane jako claims.
    /// </remarks>
    public async Task<Guid> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Email = request.Email,
            NormalizedEmail = request.Email.ToUpperInvariant(),
            UserName = request.Login,
            NormalizedUserName = request.Login.ToUpperInvariant(),
            EmailConfirmed = false,
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            PasswordHash = new PasswordHasher<User>().HashPassword(null!, request.Password),
            UserRoles = new List<UserRole>
            {
                new UserRole { Role = new Role("User") }
            }
        };

        // Dodaj Claims dla FirstName, LastName i BirthDate
        user.Claims = new List<IdentityUserClaim<Guid>>
        {
            new IdentityUserClaim<Guid>
            {
                ClaimType = "FirstName",
                ClaimValue = request.FirstName
            },
            new IdentityUserClaim<Guid>
            {
                ClaimType = "LastName",
                ClaimValue = request.LastName
            },
            new IdentityUserClaim<Guid>
            {
                ClaimType = "BirthDate",
                ClaimValue = request.BirthDate.ToString("yyyy-MM-dd")
            }
        };

        var createdUser = await userRepository.CreateAsync(user);

        return createdUser.Id;
    }
}
