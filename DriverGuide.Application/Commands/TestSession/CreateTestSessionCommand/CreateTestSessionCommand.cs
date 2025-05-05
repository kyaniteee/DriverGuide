using MediatR;

namespace DriverGuide.Application.Commands;

public class CreateTestSessionCommand : IRequest<Guid>
{
    public string? UserId { get; set; }
}
