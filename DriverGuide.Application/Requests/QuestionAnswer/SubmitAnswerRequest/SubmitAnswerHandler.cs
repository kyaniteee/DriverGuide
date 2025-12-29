using DriverGuide.Domain.Interfaces;
using MediatR;

namespace DriverGuide.Application.Requests;

public class SubmitAnswerHandler(IQuestionAnswerRepository questionAnswerRepository) : IRequestHandler<SubmitAnswerRequest, Unit>
{
    public async Task<Unit> Handle(SubmitAnswerRequest request, CancellationToken cancellationToken)
    {
        var questionAnswer = await questionAnswerRepository.GetAsync(
            qa => qa.TestSessionId == request.TestSessionId && qa.QuestionId == request.QuestionId)
            ?? throw new InvalidOperationException($"Question answer not found for TestSessionId: {request.TestSessionId}, QuestionId: {request.QuestionId}");

        questionAnswer.UserQuestionAnswer = request.UserAnswer;
        questionAnswer.EndDate = request.EndDate ?? DateTimeOffset.Now;

        await questionAnswerRepository.UpdateAsync(questionAnswer);

        return Unit.Value;
    }
}
