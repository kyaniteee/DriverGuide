using FluentValidation;

namespace DriverGuide.Application.Commands;

public class CreateTestValidator : AbstractValidator<CreateTestCommand>
{
    public CreateTestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}
