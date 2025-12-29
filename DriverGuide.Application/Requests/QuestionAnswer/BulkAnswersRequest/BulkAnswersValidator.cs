using FluentValidation;

namespace DriverGuide.Application.Requests;

public class BulkAnswersValidator : AbstractValidator<BulkAnswersRequest>
{
    public BulkAnswersValidator()
    {
        RuleFor(x => x.TestSessionId)
            .NotEmpty().WithMessage("TestSessionId jest wymagane");

        RuleFor(x => x.Answers)
            .NotNull().WithMessage("Lista odpowiedzi jest wymagana")
            .NotEmpty().WithMessage("Lista odpowiedzi nie moøe byÊ pusta");

        RuleForEach(x => x.Answers).ChildRules(answer =>
        {
            answer.RuleFor(x => x.QuestionId)
                .NotEmpty().WithMessage("QuestionId jest wymagane");

            answer.RuleFor(x => x.UserQuestionAnswer)
                .NotEmpty().WithMessage("Odpowiedü uøytkownika jest wymagana");
        });
    }
}
