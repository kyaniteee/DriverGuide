using FluentValidation;

namespace DriverGuide.Application.Commands;

public class CreateTestSessionValidator : AbstractValidator<CreateTestSessionCommand>
{
    public CreateTestSessionValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Data rozpoczêcia jest wymagana")
            .LessThanOrEqualTo(DateTimeOffset.Now.AddMinutes(5))
            .WithMessage("Data rozpoczêcia nie mo¿e byæ w przysz³oœci");

        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("Nieprawid³owa kategoria prawa jazdy");
    }
}
