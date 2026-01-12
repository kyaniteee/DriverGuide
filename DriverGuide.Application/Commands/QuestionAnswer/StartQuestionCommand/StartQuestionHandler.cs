using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Commands;

public class StartQuestionHandler(IQuestionAnswerRepository questionAnswerRepository) : IRequestHandler<StartQuestionCommand, Guid>
{
    public async Task<Guid> Handle(StartQuestionCommand request, CancellationToken cancellationToken)
    {
        var questionAnswer = new QuestionAnswer
        {
            QuestionAnswerId = Guid.NewGuid().ToString(),
            TestSessionId = request.TestSessionId,
            QuestionId = request.QuestionId,
            QuestionCategory = request.QuestionCategory,
            QuestionText = request.Question,
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
