using FluentValidation;

namespace DriverGuide.Application.Queries;

public class LoginUserValidator : AbstractValidator<LoginUserQuery>
{
    public LoginUserValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("Login jest wymagany")
            .MinimumLength(3).WithMessage("Login musi mieæ co najmniej 3 znaki");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Has³o jest wymagane")
            .MinimumLength(6).WithMessage("Has³o musi mieæ co najmniej 6 znaków");
    }
}
