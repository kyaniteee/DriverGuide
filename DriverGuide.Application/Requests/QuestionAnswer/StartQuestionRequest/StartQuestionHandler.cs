using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Requests;

public class StartQuestionHandler(IQuestionAnswerRepository questionAnswerRepository) : IRequestHandler<StartQuestionRequest, Guid>
{
    public async Task<Guid> Handle(StartQuestionRequest request, CancellationToken cancellationToken)
    {
        var questionAnswer = new QuestionAnswer
        {
            QuestionAnswerId = Guid.NewGuid().ToString(),
            TestSessionId = request.TestSessionId,
            QuestionId = request.QuestionId,
            QuestionCategory = request.QuestionCategory,
            Question = request.Question,
            CorrectQuestionAnswer = request.CorrectQuestionAnswer,
            StartDate = request.StartDate,
            QuestionLanguage = request.QuestionLanguage,
            UserQuestionAnswer = null,
            EndDate = null
        };

        await questionAnswerRepository.CreateAsync(questionAnswer);

        return Guid.Parse(questionAnswer.QuestionAnswerId);
    }
}
