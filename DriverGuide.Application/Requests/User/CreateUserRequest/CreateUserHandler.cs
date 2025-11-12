using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DriverGuide.Application.Requests;

public class CreateUserHandler(IUserRepository userRepository) : IRequestHandler<CreateUserRequest, Guid>
{
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
