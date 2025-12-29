using MediatR;

namespace DriverGuide.Application.Requests;

/// <summary>
/// Request dotyczący logowania użytkownika do systemu.
/// Implementuje wzorzec CQRS poprzez interfejs IRequest z MediatR.
/// </summary>
public class LoginUserRequest : IRequest<string>
{
    /// <summary>
    /// Login użytkownika (nazwa użytkownika lub adres email).
    /// Używany do identyfikacji użytkownika podczas procesu uwierzytelniania.
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Hasło użytkownika w postaci niezaszyfrowanej.
    /// Hasło zostanie zweryfikowane z hashem przechowywanym w bazie danych.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
