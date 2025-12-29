using MediatR;

namespace DriverGuide.Application.Commands;

/// <summary>
/// Command tworzenia nowej sesji testowej dla użytkownika.
/// Reprezentuje intencję rozpoczęcia testu egzaminacyjnego.
/// </summary>
public class CreateTestSessionCommand : IRequest<Guid>
{
    /// <summary>
    /// Identyfikator użytkownika rozpoczynającego sesję testową.
    /// Może być null dla użytkowników niezalogowanych (tryb gościa).
    /// </summary>
    public string? UserId { get; set; }
}
