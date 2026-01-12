using MediatR;

namespace DriverGuide.Application.Commands;

public class CompleteTestSessionCommand : IRequest<bool>
{
    public string TestSessionId { get; set; } = string.Empty;
    public double Result { get; set; }
}
