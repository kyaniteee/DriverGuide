using FluentValidation;

namespace DriverGuide.Application.Requests;

public class CreateTestSessionRequestValidator : AbstractValidator<CreateTestSessionRequest>
{
    public CreateTestSessionRequestValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Data rozpoczêcia jest wymagana")
            .LessThanOrEqualTo(DateTimeOffset.Now.AddMinutes(5))
            .WithMessage("Data rozpoczêcia nie mo¿e byæ w przysz³oœci");

        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("Nieprawid³owa kategoria prawa jazdy");
    }
}
