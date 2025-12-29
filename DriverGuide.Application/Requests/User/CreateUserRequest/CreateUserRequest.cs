using MediatR;

namespace DriverGuide.Application.Requests;

/// <summary>
/// Request dotyczący rejestracji nowego użytkownika w systemie.
/// Zawiera wszystkie wymagane dane do utworzenia konta użytkownika.
/// </summary>
public class CreateUserRequest : IRequest<Guid>
{
    /// <summary>
    /// Unikalna nazwa użytkownika (login) używana do logowania.
    /// Musi być unikalna w całym systemie.
    /// </summary>
    public required string Login { get; set; }

    /// <summary>
    /// Imię użytkownika.
    /// Przechowywane jako claim w tokenie JWT.
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    /// Nazwisko użytkownika.
    /// Przechowywane jako claim w tokenie JWT.
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    /// Data urodzenia użytkownika.
    /// Używana do weryfikacji minimalnego wieku (13 lat).
    /// </summary>
    public required DateOnly BirthDate { get; set; }

    /// <summary>
    /// Adres email użytkownika.
    /// Musi być unikalny i poprawny format email.
    /// Używany do komunikacji i odzyskiwania hasła.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Hasło użytkownika w postaci niezaszyfrowanej.
    /// Zostanie zahashowane przed zapisaniem do bazy danych.
    /// Musi spełniać wymogi złożoności: wielka litera, mała litera, cyfra, znak specjalny.
    /// </summary>
    public required string Password { get; set; }
}
