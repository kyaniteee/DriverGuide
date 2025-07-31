using MediatR;

namespace DriverGuide.Application.Requests;
public class LoginUserRequest : IRequest<Guid>
{
    public required string Login { get; set; }
    public required string Password { get; init; }
}
