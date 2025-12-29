using FluentValidation;

namespace DriverGuide.Application.Commands;

/// <summary>
/// Walidator dla command CreateTestSessionCommand.
/// Obecnie nie zawiera aktywnych reguł walidacji, ale może być rozszerzony w przyszłości.
/// </summary>
public class CreateTestSessionValidator : AbstractValidator<CreateTestSessionCommand>
{
    /// <summary>
    /// Inicjalizuje nową instancję walidatora CreateTestSessionValidator.
    /// Aktualnie nie definiuje żadnych reguł walidacji dla UserId.
    /// </summary>
    public CreateTestSessionValidator()
    {
    }
}
