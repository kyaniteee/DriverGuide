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
            UserName = request.Login,
            PasswordHash = new PasswordHasher<User>().HashPassword(null!, request.Password),
            UserRoles = new List<UserRole>
            {
                new UserRole { Role = new Role("User") }
            }
        };

        var createdUser = await userRepository.CreateAsync(user);

        return createdUser.Id;
    }
}
