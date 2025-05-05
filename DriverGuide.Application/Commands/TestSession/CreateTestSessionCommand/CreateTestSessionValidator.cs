using FluentValidation;

namespace DriverGuide.Application.Commands;

public class CreateTestSessionValidator : AbstractValidator<CreateTestSessionCommand>
{
    public CreateTestSessionValidator()
    {
        //RuleFor(x => x.UserId).NotEmpty();
    }
}
