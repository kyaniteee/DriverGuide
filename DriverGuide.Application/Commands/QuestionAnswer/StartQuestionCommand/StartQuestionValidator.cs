using FluentValidation;

namespace DriverGuide.Application.Commands;

public class StartQuestionValidator : AbstractValidator<StartQuestionCommand>
{
    public StartQuestionValidator()
    {
        RuleFor(x => x.TestSessionId)
            .NotEmpty().WithMessage("TestSessionId jest wymagane");

        RuleFor(x => x.QuestionId)
            .GreaterThan(0).WithMessage("QuestionId musi byæ wiêksze od 0");

        RuleFor(x => x.Question)
            .NotEmpty().WithMessage("Treœæ pytania jest wymagana");

        RuleFor(x => x.QuestionCategory)
            .IsInEnum().WithMessage("Nieprawid³owa kategoria pytania");

        RuleFor(x => x.QuestionLanguage)
            .IsInEnum().WithMessage("Nieprawid³owy jêzyk pytania");

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(DateTimeOffset.Now.AddMinutes(5))
            .WithMessage("Data rozpoczêcia nie mo¿e byæ w przysz³oœci");
    }
}
