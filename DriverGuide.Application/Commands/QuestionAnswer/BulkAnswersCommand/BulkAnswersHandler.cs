using DriverGuide.Domain.Interfaces;
using MediatR;

namespace DriverGuide.Application.Commands;

public class BulkAnswersHandler(IQuestionAnswerRepository questionAnswerRepository) : IRequestHandler<BulkAnswersCommand, Unit>
{
    public async Task<Unit> Handle(BulkAnswersCommand request, CancellationToken cancellationToken)
    {
        foreach (var answer in request.Answers)
        {
            var questionAnswer = await questionAnswerRepository.GetAsync(
                qa => qa.TestSessionId == request.TestSessionId && qa.QuestionId == answer.QuestionId);

            if (questionAnswer != null)
            {
                questionAnswer.UserQuestionAnswer = answer.UserQuestionAnswer;
                questionAnswer.EndDate = answer.EndDate;
                await questionAnswerRepository.UpdateAsync(questionAnswer);
            }
        }

        return Unit.Value;
    }
}
