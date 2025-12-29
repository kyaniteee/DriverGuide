using FluentValidation;

namespace DriverGuide.Application.Requests;

/// <summary>
/// Walidator dla żądania logowania użytkownika.
/// Sprawdza poprawność danych wejściowych przed przetworzeniem żądania.
/// </summary>
public class LoginUserValidator : AbstractValidator<LoginUserRequest>
{
    /// <summary>
    /// Inicjalizuje nową instancję walidatora LoginUserValidator.
    /// Definiuje reguły walidacji dla loginu i hasła.
    /// </summary>
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
