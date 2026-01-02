using MediatR;

namespace DriverGuide.Application.Queries;

public class GetTestSessionDetailsQuery : IRequest<TestSessionDetailsDto?>
{
    public required string TestSessionId { get; set; }
}

public class TestSessionDetailsDto
{
    public string? TestSessionId { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public double? Result { get; set; }
    public Guid? UserId { get; set; }
    public int TotalAnswers { get; set; }
    public int AnsweredQuestions { get; set; }
}
