using FluentValidation;
using DriverGuide.Domain.Interfaces;

namespace DriverGuide.Application.Requests;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    private readonly IUserRepository _userRepository;

    public CreateUserValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("Login jest wymagany")
            .MinimumLength(3).WithMessage("Login musi mieć co najmniej 3 znaki")
            .MaximumLength(50).WithMessage("Login może mieć maksymalnie 50 znaków")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Login może zawierać tylko litery, cyfry i podkreślnik")
            .MustAsync(BeUniqueLogin).WithMessage("Ten login jest już zajęty");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Imię jest wymagane")
            .MinimumLength(2).WithMessage("Imię musi mieć co najmniej 2 znaki")
            .MaximumLength(100).WithMessage("Imię może mieć maksymalnie 100 znaków")
            .Matches(@"^[a-zA-ZąćęłńóśźżĄĆĘŁŃÓŚŹŻ\s\-]+$").WithMessage("Imię może zawierać tylko litery, spacje i myślnik");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Nazwisko jest wymagane")
            .MinimumLength(2).WithMessage("Nazwisko musi mieć co najmniej 2 znaki")
            .MaximumLength(100).WithMessage("Nazwisko może mieć maksymalnie 100 znaków")
            .Matches(@"^[a-zA-ZąćęłńóśźżĄĆĘŁŃÓŚŹŻ\s\-]+$").WithMessage("Nazwisko może zawierać tylko litery, spacje i myślnik");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Data urodzenia jest wymagana")
            .Must(BeValidAge).WithMessage("Musisz mieć co najmniej 13 lat")
            .Must(BeRealisticBirthDate).WithMessage("Data urodzenia musi być pomiędzy 1900 a dzisiejszą datą");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email jest wymagany")
            .EmailAddress().WithMessage("Nieprawidłowy adres email")
            .MaximumLength(256).WithMessage("Email może mieć maksymalnie 256 znaków")
            .MustAsync(BeUniqueEmail).WithMessage("Ten adres email jest już zarejestrowany");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Hasło jest wymagane")
            .MinimumLength(8).WithMessage("Hasło musi mieć co najmniej 8 znaków")
            .MaximumLength(128).WithMessage("Hasło może mieć maksymalnie 128 znaków")
            .Matches(@"[A-Z]").WithMessage("Hasło musi zawierać co najmniej jedną wielką literę")
            .Matches(@"[a-z]").WithMessage("Hasło musi zawierać co najmniej jedną małą literę")
            .Matches(@"[0-9]").WithMessage("Hasło musi zawierać co najmniej jedną cyfrę")
            .Matches(@"[\W_]").WithMessage("Hasło musi zawierać co najmniej jeden znak specjalny");
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
