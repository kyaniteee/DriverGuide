using FluentValidation;

namespace DriverGuide.Application.Requests;

public class CompleteTestSessionValidator : AbstractValidator<CompleteTestSessionRequest>
{
    public CompleteTestSessionValidator()
    {
        RuleFor(x => x.TestSessionId)
            .NotEmpty().WithMessage("TestSessionId jest wymagane");

        RuleFor(x => x.Result)
            .GreaterThanOrEqualTo(0).WithMessage("Wynik nie mo¿e byæ ujemny")
            .LessThanOrEqualTo(100).WithMessage("Wynik nie mo¿e przekraczaæ 100%");
    }
}
