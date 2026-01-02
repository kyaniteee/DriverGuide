using DriverGuide.Domain.DTOs;
using DriverGuide.Domain.Interfaces;
using MediatR;

namespace DriverGuide.Application.Queries;

public class GetTestSummaryHandler : IRequestHandler<GetTestSummaryQuery, TestSummaryDto?>
{
    private readonly ITestSessionRepository _testSessionRepository;
    private readonly IQuestionAnswerRepository _questionAnswerRepository;
    private readonly IQuestionRepository _questionRepository;

    public GetTestSummaryHandler(
        ITestSessionRepository testSessionRepository,
        IQuestionAnswerRepository questionAnswerRepository,
        IQuestionRepository questionRepository)
    {
        _testSessionRepository = testSessionRepository;
        _questionAnswerRepository = questionAnswerRepository;
        _questionRepository = questionRepository;
    }

    public async Task<TestSummaryDto?> Handle(GetTestSummaryQuery request, CancellationToken cancellationToken)
    {
        var testSessions = await _testSessionRepository.GetAllAsync();
        var testSession = testSessions.FirstOrDefault(x => x.TestSessionId == request.TestSessionId);
        
        if (testSession == null)
            return null;

        var result = await _questionAnswerRepository.GetAllAsync();
        var questionAnswers = result.Where(x => x.TestSessionId == request.TestSessionId);
        var questions = new List<QuestionSummaryDto>();

        foreach (var qa in questionAnswers)
        {
            var question = await _questionRepository.GetByIdAsync(int.Parse(qa.QuestionId ?? "0"));
            if (question == null) continue;

            var availableAnswers = new List<string>();
            if (!string.IsNullOrWhiteSpace(question.OdpowiedzA) &&
                !string.IsNullOrWhiteSpace(question.OdpowiedzB) &&
                !string.IsNullOrWhiteSpace(question.OdpowiedzC))
            {
                availableAnswers.AddRange(new[] { question.OdpowiedzA, question.OdpowiedzB, question.OdpowiedzC });
            }
            else
            {
                availableAnswers.AddRange(new[] { "Tak", "Nie" });
            }

            questions.Add(new QuestionSummaryDto
            {
                QuestionId = qa.QuestionId ?? "",
                QuestionText = qa.Question ?? "",
                MediaFileName = question.Media,
                CorrectAnswer = qa.CorrectQuestionAnswer ?? "",
                UserAnswer = qa.UserQuestionAnswer,
                IsCorrect = qa.CorrectQuestionAnswer == qa.UserQuestionAnswer,
                AvailableAnswers = availableAnswers,
                StartDate = qa.StartDate,
                EndDate = qa.EndDate,
                Points = question.Points
            });
        }

        var correctAnswers = questions.Count(q => q.IsCorrect);
        var totalQuestions = questions.Count;

        return new TestSummaryDto
        {
            TestSessionId = testSession.TestSessionId ?? "",
            Category = questionAnswers.FirstOrDefault()?.QuestionCategory ?? Domain.Enums.LicenseCategory.B,
            Result = testSession.Result,
            StartDate = testSession.StartDate,
            EndDate = testSession.EndDate,
            CorrectAnswers = correctAnswers,
            TotalQuestions = totalQuestions,
            Questions = questions.OrderBy(q => q.StartDate).ToList()
        };
    }
}
