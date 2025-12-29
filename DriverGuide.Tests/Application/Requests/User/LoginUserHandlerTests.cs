using DriverGuide.Application.Requests;
using DriverGuide.Application.Services;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using FluentAssertions;
using NSubstitute;

namespace DriverGuide.Tests.Application.Requests.User;

/// <summary>
/// Klasa testowa dla LoginUserHandler.
/// Testuje proces uwierzytelniania u¿ytkownika, weryfikacji has³a i generowania tokenu JWT.
/// Pokrywa scenariusze sukcesu i b³êdów uwierzytelniania.
/// </summary>
public class LoginUserHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly LoginUserHandler _handler;

    /// <summary>
    /// Konstruktor inicjalizuj¹cy mocki repozytorium i generatora JWT oraz instancjê handlera.
    /// </summary>
    public LoginUserHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        _handler = new LoginUserHandler(_userRepository, _jwtTokenGenerator);
    }

    /// <summary>
    /// Test weryfikuj¹cy poprawne logowanie u¿ytkownika z prawid³owymi danymi uwierzytelniaj¹cymi.
    /// Sprawdza czy handler:
    /// - Pobiera u¿ytkownika z rolami i claims
    /// - Weryfikuje has³o
    /// - Generuje i zwraca token JWT
    /// </summary>
    /// <remarks>
    /// Test scenariusza happy path dla logowania.
    /// Mockuje wszystkie zale¿noœci aby zwraca³y oczekiwane wartoœci.
    /// </remarks>
    [Fact]
    public async Task Handle_ValidCredentials_ShouldReturnToken()
    {
        var request = new LoginUserRequest
        {
            Login = "testuser",
            Password = "Password123!"
        };

        var user = new DriverGuide.Domain.Models.User
        {
            Id = Guid.NewGuid(),
            UserName = "testuser",
            Email = "test@example.com",
            UserRoles = new List<UserRole>()
        };

        var expectedToken = "generated-jwt-token";

        _userRepository.GetWithRolesAndClaimsAsync(request.Login)
            .Returns(Task.FromResult<DriverGuide.Domain.Models.User?>(user));

        _userRepository.VerifyPasswordAsync(user, request.Password)
            .Returns(Task.FromResult(true));

        _jwtTokenGenerator.GenerateToken(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<List<string>>(),
            Arg.Any<List<System.Security.Claims.Claim>>())
            .Returns(expectedToken);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().Be(expectedToken);
        await _userRepository.Received(1).GetWithRolesAndClaimsAsync(request.Login);
        await _userRepository.Received(1).VerifyPasswordAsync(user, request.Password);
    }

    /// <summary>
    /// Test weryfikuj¹cy obs³ugê scenariusza gdy u¿ytkownik nie istnieje w systemie.
    /// Sprawdza czy handler rzuca wyj¹tek UnauthorizedAccessException z odpowiednim komunikatem.
    /// </summary>
    /// <remarks>
    /// Test security - sprawdza czy system nie ujawnia czy u¿ytkownik istnieje czy nie.
    /// Komunikat b³êdu jest ogólny: "Nieprawid³owy login lub has³o".
    /// </remarks>
    [Fact]
    public async Task Handle_UserNotFound_ShouldThrowUnauthorizedException()
    {
        var request = new LoginUserRequest
        {
            Login = "nonexistent",
            Password = "Password123!"
        };

        _userRepository.GetWithRolesAndClaimsAsync(request.Login)
            .Returns(Task.FromResult<DriverGuide.Domain.Models.User?>(null));

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _handler.Handle(request, CancellationToken.None));
    }

    /// <summary>
    /// Test weryfikuj¹cy obs³ugê scenariusza z nieprawid³owym has³em.
    /// Sprawdza czy handler rzuca wyj¹tek UnauthorizedAccessException gdy has³o nie pasuje.
    /// </summary>
    /// <remarks>
    /// Test security - weryfikuje ¿e nieprawid³owe has³o jest odrzucane.
    /// Komunikat b³êdu jest taki sam jak dla nieistniej¹cego u¿ytkownika (zapobiega user enumeration).
    /// </remarks>
    [Fact]
    public async Task Handle_InvalidPassword_ShouldThrowUnauthorizedException()
    {
        var request = new LoginUserRequest
        {
            Login = "testuser",
            Password = "WrongPassword!"
        };

        var user = new DriverGuide.Domain.Models.User
        {
            Id = Guid.NewGuid(),
            UserName = "testuser",
            Email = "test@example.com"
        };

        _userRepository.GetWithRolesAndClaimsAsync(request.Login)
            .Returns(Task.FromResult<DriverGuide.Domain.Models.User?>(user));

        _userRepository.VerifyPasswordAsync(user, request.Password)
            .Returns(Task.FromResult(false));

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _handler.Handle(request, CancellationToken.None));
    }
}
