using DriverGuide.Domain.Interfaces;
using MediatR;

namespace DriverGuide.Application.Queries;

public class GetTestSessionDetailsHandler : IRequestHandler<GetTestSessionDetailsQuery, TestSessionDetailsDto?>
{
    private readonly ITestSessionRepository _testSessionRepository;
    private readonly IQuestionAnswerRepository _questionAnswerRepository;

    public GetTestSessionDetailsHandler(
        ITestSessionRepository testSessionRepository,
        IQuestionAnswerRepository questionAnswerRepository)
    {
        _testSessionRepository = testSessionRepository;
        _questionAnswerRepository = questionAnswerRepository;
    }

    public async Task<TestSessionDetailsDto?> Handle(GetTestSessionDetailsQuery request, CancellationToken cancellationToken)
    {
        var session = await _testSessionRepository.GetByIdAsync(request.TestSessionId);
        if (session == null)
            return null;

        var answers = await _questionAnswerRepository.GetByTestSessionIdAsync(request.TestSessionId);

        return new TestSessionDetailsDto
        {
            TestSessionId = session.TestSessionId,
            StartDate = session.StartDate,
            EndDate = session.EndDate,
            Result = session.Result,
            UserId = session.UserId,
            TotalAnswers = answers?.Count ?? 0,
            AnsweredQuestions = answers?.Count(a => !string.IsNullOrEmpty(a.UserQuestionAnswer)) ?? 0
        };
    }
}
