using MediatR;

namespace DriverGuide.Application.Requests;

public class CreateUserRequest : IRequest<Guid>
{
    public string? Login { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}
