namespace DriverGuide.Application.Requests;

public class CompleteTestSessionRequest
{
    public string TestSessionId { get; set; } = string.Empty;
    public double Result { get; set; }
}
