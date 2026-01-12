using FluentValidation;

namespace DriverGuide.Application.Commands;

public class CompleteTestSessionValidator : AbstractValidator<CompleteTestSessionCommand>
{
    public CompleteTestSessionValidator()
    {
        RuleFor(x => x.TestSessionId)
            .NotEmpty().WithMessage("ID sesji testowej jest wymagane");

        RuleFor(x => x.Result)
            .GreaterThanOrEqualTo(0).WithMessage("Wynik nie mo¿e byæ ujemny")
            .LessThanOrEqualTo(100).WithMessage("Wynik nie mo¿e przekraczaæ 100");
    }
}
