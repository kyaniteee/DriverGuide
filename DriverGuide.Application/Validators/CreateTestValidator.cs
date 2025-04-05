using DriverGuide.Application.UseCases.Tests;
using FluentValidation;

namespace DriverGuide.Application.Validators
{
    public class CreateTestValidator : AbstractValidator<CreateTestCommand>
    {
        public CreateTestValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
