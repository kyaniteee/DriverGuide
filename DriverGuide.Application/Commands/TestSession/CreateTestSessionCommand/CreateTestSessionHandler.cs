using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Commands;

/// <summary>
/// Handler odpowiedzialny za tworzenie nowej sesji testowej w systemie.
/// Weryfikuje istnienie użytkownika i tworzy nową sesję testową z aktualnym czasem rozpoczęcia.
/// </summary>
/// <param name="testSessionRepository">Repozytorium sesji testowych do zapisu danych.</param>
/// <param name="userRepository">Repozytorium użytkowników do weryfikacji istnienia użytkownika.</param>
public class CreateTestSessionHandler(ITestSessionRepository testSessionRepository, IUserRepository userRepository) : IRequestHandler<CreateTestSessionCommand, Guid>
{
    /// <summary>
    /// Obsługuje żądanie utworzenia nowej sesji testowej.
    /// Weryfikuje czy użytkownik istnieje, tworzy nową sesję z unikalnym ID i aktualnym czasem rozpoczęcia.
    /// </summary>
    /// <param name="request">Obiekt żądania zawierający ID użytkownika.</param>
    /// <param name="cancellationToken">Token anulowania operacji asynchronicznej.</param>
    /// <returns>
    /// GUID nowo utworzonej sesji testowej.
    /// </returns>
    /// <exception cref="Exception">
    /// Rzucany gdy użytkownik o podanym ID nie zostanie znaleziony w systemie.
    /// </exception>
    /// <remarks>
    /// Sesja testowa jest tworzona z:
    /// - Nowym unikalnym GUID jako identyfikatorem
    /// - Aktualnym czasem jako datą rozpoczęcia
    /// - Powiązaniem z użytkownikiem
    /// - Pustymi wartościami dla EndDate i Result (wypełniane po zakończeniu testu)
    /// </remarks>
    public async Task<Guid> Handle(CreateTestSessionCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByGuidAsync(Guid.Parse(request.UserId!));
        if (user == null)
            throw new Exception("User not found");

        var testSession = new TestSession
        {
            StartDate = DateTime.Now,
            UserId = user.Id,
            TestSessionId = Guid.NewGuid().ToString(),
        };

        await testSessionRepository.CreateAsync(testSession);

        return Guid.Parse(testSession.TestSessionId);
    }
}
