using DriverGuide.Application.Requests;
using FluentValidation.TestHelper;

namespace DriverGuide.Tests.Application.Requests.User;

/// <summary>
/// Klasa testowa dla LoginUserValidator.
/// Testuje regu³y walidacji danych uwierzytelniaj¹cych u¿ytkownika (login i has³o).
/// Weryfikuje czy wymagania bezpieczeñstwa s¹ egzekwowane przed prób¹ logowania.
/// </summary>
public class LoginUserValidatorTests
{
    private readonly LoginUserValidator _validator;

    /// <summary>
    /// Konstruktor inicjalizuj¹cy instancjê walidatora.
    /// Tworzy now¹ instancjê dla ka¿dego testu zapewniaj¹c izolacjê.
    /// </summary>
    public LoginUserValidatorTests()
    {
        _validator = new LoginUserValidator();
    }

    /// <summary>
    /// Test weryfikuj¹cy ¿e poprawne dane logowania przechodz¹ walidacjê bez b³êdów.
    /// Sprawdza scenariusz happy path - login i has³o spe³niaj¹ wszystkie wymagania.
    /// </summary>
    /// <remarks>
    /// Podstawowy test pozytywny dla walidatora logowania.
    /// U¿ywa danych testowych spe³niaj¹cych wszystkie regu³y (min. d³ugoœæ).
    /// </remarks>
    [Fact]
    public async Task Validate_ValidRequest_ShouldNotHaveValidationError()
    {
        var request = new LoginUserRequest
        {
            Login = "validuser",
            Password = "ValidPassword123"
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Test parametryzowany weryfikuj¹cy walidacjê pola Login.
    /// Sprawdza ró¿ne nieprawid³owe wartoœci i odpowiadaj¹ce im komunikaty b³êdów.
    /// </summary>
    /// <param name="login">Testowana wartoœæ loginu (pusty string lub za krótka).</param>
    /// <param name="expectedError">Oczekiwany komunikat b³êdu walidacji po polsku.</param>
    /// <remarks>
    /// Testuje dwie regu³y walidacji:
    /// - NotEmpty: Login nie mo¿e byæ pusty
    /// - MinimumLength(3): Login musi mieæ co najmniej 3 znaki
    /// U¿ywa Theory z InlineData dla parametryzacji testów.
    /// </remarks>
    [Theory]
    [InlineData("", "Login jest wymagany")]
    [InlineData("ab", "Login musi mieæ co najmniej 3 znaki")]
    public async Task Validate_InvalidLogin_ShouldHaveValidationError(string login, string expectedError)
    {
        var request = new LoginUserRequest
        {
            Login = login,
            Password = "ValidPassword"
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Login)
            .WithErrorMessage(expectedError);
    }

    /// <summary>
    /// Test parametryzowany weryfikuj¹cy walidacjê pola Password.
    /// Sprawdza ró¿ne nieprawid³owe has³a i odpowiadaj¹ce im komunikaty b³êdów.
    /// </summary>
    /// <param name="password">Testowane has³o (puste lub za krótkie).</param>
    /// <param name="expectedError">Oczekiwany komunikat b³êdu walidacji po polsku.</param>
    /// <remarks>
    /// Testuje dwie regu³y walidacji:
    /// - NotEmpty: Has³o nie mo¿e byæ puste
    /// - MinimumLength(6): Has³o musi mieæ co najmniej 6 znaków
    /// Wymóg 6 znaków jest minimalnym bezpiecznym wymogiem dla logowania.
    /// </remarks>
    [Theory]
    [InlineData("", "Has³o jest wymagane")]
    [InlineData("short", "Has³o musi mieæ co najmniej 6 znaków")]
    public async Task Validate_InvalidPassword_ShouldHaveValidationError(string password, string expectedError)
    {
        var request = new LoginUserRequest
        {
            Login = "validuser",
            Password = password
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage(expectedError);
    }
}
