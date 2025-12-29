using FluentValidation;

namespace DriverGuide.Application.Requests;

public class SubmitAnswerValidator : AbstractValidator<SubmitAnswerRequest>
{
    public SubmitAnswerValidator()
    {
        RuleFor(x => x.TestSessionId)
            .NotEmpty().WithMessage("TestSessionId jest wymagane");

        RuleFor(x => x.QuestionId)
            .NotEmpty().WithMessage("QuestionId jest wymagane");

        RuleFor(x => x.UserAnswer)
            .NotEmpty().WithMessage("Odpowiedü uøytkownika jest wymagana");
    }
}
