using FluentValidation;
using DriverGuide.Domain.Interfaces;

namespace DriverGuide.Application.Commands;

public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public CreateUserValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("Login jest wymagany")
            .MinimumLength(3).WithMessage("Login musi mieæ co najmniej 3 znaki")
            .MaximumLength(50).WithMessage("Login mo¿e mieæ maksymalnie 50 znaków")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Login mo¿e zawieraæ tylko litery, cyfry i podkreœlnik")
            .MustAsync(BeUniqueLogin).WithMessage("Ten login jest ju¿ zajêty");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Imiê jest wymagane")
            .MinimumLength(2).WithMessage("Imiê musi mieæ co najmniej 2 znaki")
            .MaximumLength(100).WithMessage("Imiê mo¿e mieæ maksymalnie 100 znaków")
            .Matches(@"^[a-zA-Z¹æê³ñóœŸ¿¥ÆÊ£ÑÓŒ¯\s\-]+$").WithMessage("Imiê mo¿e zawieraæ tylko litery, spacje i myœlnik");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Nazwisko jest wymagane")
            .MinimumLength(2).WithMessage("Nazwisko musi mieæ co najmniej 2 znaki")
            .MaximumLength(100).WithMessage("Nazwisko mo¿e mieæ maksymalnie 100 znaków")
            .Matches(@"^[a-zA-Z¹æê³ñóœŸ¿¥ÆÊ£ÑÓŒ¯\s\-]+$").WithMessage("Nazwisko mo¿e zawieraæ tylko litery, spacje i myœlnik");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Data urodzenia jest wymagana")
            .Must(BeValidAge).WithMessage("Musisz mieæ co najmniej 13 lat")
            .Must(BeRealisticBirthDate).WithMessage("Data urodzenia musi byæ pomiêdzy 1900 a dzisiejsz¹ dat¹");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email jest wymagany")
            .EmailAddress().WithMessage("Nieprawid³owy adres email")
            .MaximumLength(256).WithMessage("Email mo¿e mieæ maksymalnie 256 znaków")
            .MustAsync(BeUniqueEmail).WithMessage("Ten adres email jest ju¿ zarejestrowany");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Has³o jest wymagane")
            .MinimumLength(8).WithMessage("Has³o musi mieæ co najmniej 8 znaków")
            .MaximumLength(128).WithMessage("Has³o mo¿e mieæ maksymalnie 128 znaków")
            .Matches(@"[A-Z]").WithMessage("Has³o musi zawieraæ co najmniej jedn¹ wielk¹ literê")
            .Matches(@"[a-z]").WithMessage("Has³o musi zawieraæ co najmniej jedn¹ ma³¹ literê")
            .Matches(@"[0-9]").WithMessage("Has³o musi zawieraæ co najmniej jedn¹ cyfrê")
            .Matches(@"[\W_]").WithMessage("Has³o musi zawieraæ co najmniej jeden znak specjalny");
    }

    private async Task<bool> BeUniqueLogin(string login, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(x => x.UserName == login, useNoTracking: true);
        return user == null;
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(x => x.Email == email, useNoTracking: true);
        return user == null;
    }

    private bool BeValidAge(DateOnly birthDate)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - birthDate.Year;
        if (birthDate > today.AddYears(-age)) age--;
        return age >= 13;
    }

    private bool BeRealisticBirthDate(DateOnly birthDate)
    {
        var minDate = new DateOnly(1900, 1, 1);
        var maxDate = DateOnly.FromDateTime(DateTime.Today);
        return birthDate >= minDate && birthDate <= maxDate;
    }
}
