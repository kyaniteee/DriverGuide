using FluentValidation;

namespace DriverGuide.Application.Requests;

public class SubmitAnswerValidator : AbstractValidator<SubmitAnswerRequest>
{
    public SubmitAnswerValidator()
    {
        RuleFor(x => x.TestSessionId)
            .NotEmpty().WithMessage("TestSessionId jest wymagane");

        RuleFor(x => x.QuestionId)
            .GreaterThan(0).WithMessage("QuestionId jest wymagana");

        RuleFor(x => x.UserAnswer)
            .NotEmpty().WithMessage("Odpowiedü uøytkownika jest wymagana");
    }
}
