using MediatR;

namespace DriverGuide.Application.Queries;

public class LoginUserQuery : IRequest<string>
{
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
