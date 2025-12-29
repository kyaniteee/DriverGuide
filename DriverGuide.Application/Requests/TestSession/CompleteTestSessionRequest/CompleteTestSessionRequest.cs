using MediatR;

namespace DriverGuide.Application.Requests;

public class CompleteTestSessionRequest : IRequest<Unit>
{
    public string TestSessionId { get; set; } = string.Empty;
    public double Result { get; set; }
}
