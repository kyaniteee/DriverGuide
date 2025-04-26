using FluentValidation;

namespace DriverGuide.Application.UseCases
{
    public class CreateTestValidator : AbstractValidator<CreateTestCommand>
    {
        public CreateTestValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
