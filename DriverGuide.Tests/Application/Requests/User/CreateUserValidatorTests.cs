using DriverGuide.Application.Requests;
using FluentValidation.TestHelper;
using NSubstitute;

namespace DriverGuide.Tests.Application.Requests.User;

/// <summary>
/// Klasa testowa dla CreateUserValidator.
/// Testuje wszystkie regu³y walidacji dla ¿¹dania rejestracji u¿ytkownika.
/// Wykorzystuje FluentValidation.TestHelper do testowania walidatorów.
/// </summary>
public class CreateUserValidatorTests
{
    private readonly CreateUserValidator _validator;
    private readonly DriverGuide.Domain.Interfaces.IUserRepository _userRepository;

    /// <summary>
    /// Konstruktor inicjalizuj¹cy mock repozytorium i instancjê walidatora.
    /// Mock repozytorium jest potrzebny dla asynchronicznych regu³ walidacji (BeUniqueLogin, BeUniqueEmail).
    /// </summary>
    public CreateUserValidatorTests()
    {
        _userRepository = Substitute.For<DriverGuide.Domain.Interfaces.IUserRepository>();
        _validator = new CreateUserValidator(_userRepository);
    }

    /// <summary>
    /// Test weryfikuj¹cy ¿e poprawne dane przechodz¹ walidacjê.
    /// Sprawdza scenariusz happy path - wszystkie dane s¹ zgodne z wymaganiami.
    /// </summary>
    /// <remarks>
    /// Mockuje repozytorium aby zwraca³o null (login i email nie istniej¹ w bazie).
    /// Jest to podstawowy test pozytywny dla walidatora.
    /// </remarks>
    [Fact]
    public async Task Validate_ValidRequest_ShouldNotHaveValidationError()
    {
        _userRepository.GetAsync(Arg.Any<System.Linq.Expressions.Expression<Func<DriverGuide.Domain.Models.User, bool>>>(), Arg.Any<bool>())
            .Returns(Task.FromResult<DriverGuide.Domain.Models.User?>(null));

        var request = new CreateUserRequest
        {
            Login = "validuser",
            FirstName = "John",
            LastName = "Doe",
            BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-20)),
            Email = "john@example.com",
            Password = "ValidPass123!"
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Test parametryzowany weryfikuj¹cy walidacjê pola Login.
    /// Testuje ró¿ne nieprawid³owe wartoœci loginu i sprawdza odpowiednie komunikaty b³êdów.
    /// </summary>
    /// <param name="login">Testowana wartoœæ loginu.</param>
    /// <param name="expectedError">Oczekiwany komunikat b³êdu walidacji.</param>
    /// <remarks>
    /// U¿ywa Theory i InlineData do testowania wielu przypadków jedn¹ metod¹.
    /// Testuje dwie regu³y: NotEmpty i MinimumLength.
    /// </remarks>
    [Theory]
    [InlineData("", "Login jest wymagany")]
    [InlineData("ab", "Login musi mieæ co najmniej 3 znaki")]
    public async Task Validate_InvalidLogin_ShouldHaveValidationError(string login, string expectedError)
    {
        var request = new CreateUserRequest
        {
            Login = login,
            FirstName = "John",
            LastName = "Doe",
            BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-20)),
            Email = "john@example.com",
            Password = "ValidPass123!"
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Login)
            .WithErrorMessage(expectedError);
    }

    /// <summary>
    /// Test parametryzowany weryfikuj¹cy walidacjê pola FirstName.
    /// Sprawdza czy imiê spe³nia wymagania minimalnej d³ugoœci.
    /// </summary>
    /// <param name="firstName">Testowana wartoœæ imienia.</param>
    /// <param name="expectedError">Oczekiwany komunikat b³êdu walidacji.</param>
    [Theory]
    [InlineData("", "Imiê jest wymagane")]
    [InlineData("A", "Imiê musi mieæ co najmniej 2 znaki")]
    public async Task Validate_InvalidFirstName_ShouldHaveValidationError(string firstName, string expectedError)
    {
        var request = new CreateUserRequest
        {
            Login = "validuser",
            FirstName = firstName,
            LastName = "Doe",
            BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-20)),
            Email = "john@example.com",
            Password = "ValidPass123!"
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.FirstName)
            .WithErrorMessage(expectedError);
    }

    /// <summary>
    /// Test parametryzowany weryfikuj¹cy walidacjê pola Email.
    /// Sprawdza czy email jest w poprawnym formacie i jest wymagany.
    /// </summary>
    /// <param name="email">Testowana wartoœæ emaila.</param>
    /// <param name="expectedError">Oczekiwany komunikat b³êdu walidacji.</param>
    /// <remarks>
    /// Testuje regu³y: NotEmpty i EmailAddress z FluentValidation.
    /// </remarks>
    [Theory]
    [InlineData("invalidemail", "Nieprawid³owy adres email")]
    [InlineData("", "Email jest wymagany")]
    public async Task Validate_InvalidEmail_ShouldHaveValidationError(string email, string expectedError)
    {
        var request = new CreateUserRequest
        {
            Login = "validuser",
            FirstName = "John",
            LastName = "Doe",
            BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-20)),
            Email = email,
            Password = "ValidPass123!"
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    /// <summary>
    /// Test parametryzowany weryfikuj¹cy z³o¿one regu³y walidacji has³a.
    /// Sprawdza wymagania dotycz¹ce d³ugoœci, wielkich liter, ma³ych liter, cyfr i znaków specjalnych.
    /// </summary>
    /// <param name="password">Testowane has³o.</param>
    /// <param name="expectedError">Oczekiwany komunikat b³êdu (mo¿e byæ nieu¿ywany gdy sprawdzamy tylko obecnoœæ b³êdu).</param>
    /// <remarks>
    /// Testuje politykê bezpieczeñstwa hase³ zgodn¹ z OWASP:
    /// - Minimum 8 znaków
    /// - Wielka litera
    /// - Ma³a litera
    /// - Cyfra
    /// - Znak specjalny
    /// </remarks>
    [Theory]
    [InlineData("", "Has³o jest wymagane")]
    [InlineData("short", "Has³o musi mieæ co najmniej 8 znaków")]
    [InlineData("nouppercas1!", "Has³o musi zawieraæ co najmniej jedn¹ wielk¹ literê")]
    [InlineData("NOLOWERCASE1!", "Has³o musi zawieraæ co najmniej jedn¹ ma³¹ literê")]
    [InlineData("NoNumber!", "Has³o musi zawieraæ co najmniej jedn¹ cyfrê")]
    [InlineData("NoSpecial1", "Has³o musi zawieraæ co najmniej jeden znak specjalny")]
    public async Task Validate_InvalidPassword_ShouldHaveValidationError(string password, string expectedError)
    {
        var request = new CreateUserRequest
        {
            Login = "validuser",
            FirstName = "John",
            LastName = "Doe",
            BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-20)),
            Email = "john@example.com",
            Password = password
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    /// <summary>
    /// Test weryfikuj¹cy walidacjê minimalnego wieku u¿ytkownika.
    /// Sprawdza czy system odrzuca rejestracjê u¿ytkowników poni¿ej 13 roku ¿ycia (COPPA compliance).
    /// </summary>
    /// <remarks>
    /// Testuje niestandardow¹ regu³ê walidacji BeValidAge.
    /// Wymóg 13 lat jest zwi¹zany z przepisami dotycz¹cymi przetwarzania danych osobowych dzieci.
    /// </remarks>
    [Fact]
    public async Task Validate_AgeTooYoung_ShouldHaveValidationError()
    {
        var request = new CreateUserRequest
        {
            Login = "validuser",
            FirstName = "John",
            LastName = "Doe",
            BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-10)),
            Email = "john@example.com",
            Password = "ValidPass123!"
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.BirthDate)
            .WithErrorMessage("Musisz mieæ co najmniej 13 lat");
    }
}
