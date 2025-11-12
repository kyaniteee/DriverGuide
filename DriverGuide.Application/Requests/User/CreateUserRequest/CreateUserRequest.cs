using MediatR;

namespace DriverGuide.Application.Requests;

public class CreateUserRequest : IRequest<Guid>
{
    public required string Login { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateOnly BirthDate { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
