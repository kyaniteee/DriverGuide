using FluentValidation;

namespace DriverGuide.Application.Requests;

public class LoginUserValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("Login jest wymagany")
            .MinimumLength(3).WithMessage("Login musi mieć co najmniej 3 znaki");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Hasło jest wymagane")
            .MinimumLength(6).WithMessage("Hasło musi mieć co najmniej 6 znaków");
    }
}
