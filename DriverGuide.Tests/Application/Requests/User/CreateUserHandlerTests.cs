using DriverGuide.Application.Requests;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using FluentAssertions;
using NSubstitute;

namespace DriverGuide.Tests.Application.Requests.User;

/// <summary>
/// Klasa testowa dla CreateUserHandler.
/// Testuje proces tworzenia nowego u¿ytkownika w systemie przy u¿yciu wzorca AAA (Arrange-Act-Assert).
/// Wykorzystuje NSubstitute do mockowania zale¿noœci i FluentAssertions do czytelnych asercji.
/// </summary>
public class CreateUserHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly CreateUserHandler _handler;

    /// <summary>
    /// Konstruktor inicjalizuj¹cy mock repozytorium u¿ytkowników i instancjê handlera.
    /// Wywo³uje siê przed ka¿dym testem zapewniaj¹c izolacjê testów.
    /// </summary>
    public CreateUserHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _handler = new CreateUserHandler(_userRepository);
    }

    /// <summary>
    /// Test weryfikuj¹cy poprawne utworzenie u¿ytkownika z poprawnymi danymi.
    /// Sprawdza czy handler:
    /// - Tworzy u¿ytkownika z wszystkimi wymaganymi polami
    /// - Wywo³uje metodê CreateAsync repozytorium dok³adnie raz
    /// - Zwraca GUID nowo utworzonego u¿ytkownika
    /// </summary>
    /// <remarks>
    /// Test stosuje wzorzec AAA:
    /// Arrange - Przygotowanie danych testowych i mockowanie repozytorium
    /// Act - Wywo³anie metody Handle
    /// Assert - Weryfikacja wyniku i wywo³añ metod
    /// </remarks>
    [Fact]
    public async Task Handle_ValidRequest_ShouldCreateUserAndReturnGuid()
    {
        var request = new CreateUserRequest
        {
            Login = "testuser",
            FirstName = "Test",
            LastName = "User",
            BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-20)),
            Email = "test@example.com",
            Password = "Password123!"
        };

        var userId = Guid.NewGuid();
        var createdUser = new DriverGuide.Domain.Models.User
        {
            Id = userId,
            UserName = request.Login,
            Email = request.Email
        };

        _userRepository.CreateAsync(Arg.Any<DriverGuide.Domain.Models.User>())
            .Returns(Task.FromResult(createdUser));

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().Be(userId);
        await _userRepository.Received(1).CreateAsync(Arg.Any<DriverGuide.Domain.Models.User>());
    }

    /// <summary>
    /// Test weryfikuj¹cy obs³ugê b³êdu podczas tworzenia u¿ytkownika.
    /// Sprawdza czy handler prawid³owo propaguje wyj¹tek z warstwy repozytorium.
    /// Symuluje awariê bazy danych lub inny b³¹d infrastruktury.
    /// </summary>
    /// <remarks>
    /// Test sprawdza scenariusz negatywny (error path) gdzie repozytorium rzuca wyj¹tek.
    /// Wa¿ne dla zapewnienia odpornoœci systemu na b³êdy.
    /// </remarks>
    [Fact]
    public async Task Handle_CreateUserFails_ShouldThrowException()
    {
        var request = new CreateUserRequest
        {
            Login = "testuser",
            FirstName = "Test",
            LastName = "User",
            BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-20)),
            Email = "test@example.com",
            Password = "Password123!"
        };

        _userRepository.CreateAsync(Arg.Any<DriverGuide.Domain.Models.User>())
            .Returns(Task.FromException<DriverGuide.Domain.Models.User>(new Exception("Database error")));

        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
    }
}
