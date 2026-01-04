using MediatR;

namespace DriverGuide.Application.Requests;

public class SubmitAnswerRequest : IRequest<Unit>
{
    public string TestSessionId { get; set; } = string.Empty;
    public int QuestionId { get; set; }
    public string UserAnswer { get; set; } = string.Empty;
    public DateTimeOffset? EndDate { get; set; }
}
