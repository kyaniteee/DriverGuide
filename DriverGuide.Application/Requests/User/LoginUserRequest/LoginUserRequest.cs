using MediatR;

namespace DriverGuide.Application.Requests;

public class LoginUserRequest : IRequest<string>
{
    public required string Login { get; set; }
    public required string Password { get; set; }
}
