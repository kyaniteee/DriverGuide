using FluentValidation;
using MediatR;

namespace DriverGuide.Application.Behaviors;

/// <summary>
/// Behavior przetwarzania pipeline MediatR wykonuj¹cy automatyczn¹ walidacjê ¿¹dañ.
/// Dzia³a jako middleware przed wykonaniem w³aœciwego handlera ¿¹dania.
/// Wykorzystuje walidatory FluentValidation do sprawdzenia poprawnoœci danych wejœciowych.
/// </summary>
/// <typeparam name="TRequest">Typ ¿¹dania implementuj¹cy IRequest.</typeparam>
/// <typeparam name="TResponse">Typ odpowiedzi zwracanej przez handler.</typeparam>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    /// <summary>
    /// Inicjalizuje now¹ instancjê ValidationBehavior.
    /// </summary>
    /// <param name="validators">
    /// Kolekcja walidatorów FluentValidation dla danego typu ¿¹dania.
    /// Wszystkie walidatory s¹ automatycznie wstrzykiwane przez kontener DI.
    /// </param>
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <summary>
    /// Obs³uguje ¿¹danie w pipeline MediatR z automatyczn¹ walidacj¹.
    /// Jeœli istniej¹ walidatory dla danego ¿¹dania, wykonuje walidacjê przed przekazaniem do handlera.
    /// W przypadku b³êdów walidacji rzuca wyj¹tek ValidationException z list¹ b³êdów.
    /// </summary>
    /// <param name="request">Obiekt ¿¹dania do zwalidowania.</param>
    /// <param name="next">Delegat do nastêpnego elementu w pipeline (zwykle w³aœciwy handler).</param>
    /// <param name="cancellationToken">Token anulowania operacji asynchronicznej.</param>
    /// <returns>
    /// OdpowiedŸ typu TResponse jeœli walidacja przebieg³a pomyœlnie.
    /// </returns>
    /// <exception cref="ValidationException">
    /// Rzucany gdy walidacja nie powiedzie siê. 
    /// Zawiera listê wszystkich b³êdów walidacji ze wszystkich walidatorów.
    /// </exception>
    /// <remarks>
    /// Jeœli nie ma zarejestrowanych walidatorów, ¿¹danie jest przekazywane bezpoœrednio do handlera.
    /// Wszystkie b³êdy z wszystkich walidatorów s¹ agregowane przed rzuceniem wyj¹tku.
    /// </remarks>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Any())
        {
            throw new ValidationException(failures);
        }

        return await next();
    }
}
