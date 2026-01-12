using MediatR;
using DriverGuide.Domain.Models;

namespace DriverGuide.Application.Commands;

public class BulkAnswersCommand : IRequest<Unit>
{
    public string TestSessionId { get; set; } = string.Empty;
    public List<BulkAnswerItem> Answers { get; set; } = new();
}
