using FluentValidation;

namespace DriverGuide.Application.Commands;

public class BulkAnswersValidator : AbstractValidator<BulkAnswersCommand>
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
                .GreaterThan(0).WithMessage("QuestionId musi byÊ wiÍksze od 0");

            answer.RuleFor(x => x.UserQuestionAnswer)
                .NotEmpty().WithMessage("Odpowiedü uøytkownika jest wymagana");
        });
    }
}
