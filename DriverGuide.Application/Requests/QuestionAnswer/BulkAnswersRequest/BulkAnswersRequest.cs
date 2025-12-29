using MediatR;
using DriverGuide.Domain.Models;

namespace DriverGuide.Application.Requests;

public class BulkAnswersRequest : IRequest<Unit>
{
    public string TestSessionId { get; set; } = string.Empty;
    public List<BulkAnswerItem> Answers { get; set; } = new();
}
