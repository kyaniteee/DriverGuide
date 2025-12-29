using DriverGuide.Application.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DriverGuide.Tests.Application.Services;

/// <summary>
/// Klasa testowa dla JwtTokenGenerator.
/// Testuje proces generowania tokenów JWT, weryfikuje zawartoœæ tokenów i ich konfiguracjê.
/// Sprawdza czy tokeny zawieraj¹ wszystkie wymagane claims, maj¹ poprawny czas wygaœniêcia i s¹ prawid³owo podpisane.
/// </summary>
public class JwtTokenGeneratorTests
{
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Konstruktor inicjalizuj¹cy konfiguracjê JWT i instancjê generatora.
    /// Tworzy in-memory configuration z testowymi ustawieniami JWT.
    /// </summary>
    /// <remarks>
    /// U¿ywa ConfigurationBuilder z AddInMemoryCollection do symulacji appsettings.json.
    /// Klucz SecretKey musi byæ wystarczaj¹co d³ugi dla HMAC SHA256.
    /// </remarks>
    public JwtTokenGeneratorTests()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            { "JwtSettings:SecretKey", "ThisIsAVerySecureSecretKeyForJwtTokenGeneration123456789" },
            { "JwtSettings:Issuer", "TestIssuer" },
            { "JwtSettings:Audience", "TestAudience" },
            { "JwtSettings:ExpiryMinutes", "60" }
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        _tokenGenerator = new JwtTokenGenerator(_configuration);
    }

    /// <summary>
    /// Test weryfikuj¹cy ¿e generator zwraca niepusty token JWT dla poprawnych danych wejœciowych.
    /// Podstawowy test sprawdzaj¹cy czy token w ogóle jest generowany.
    /// </summary>
    [Fact]
    public void GenerateToken_ValidInput_ShouldReturnToken()
    {
        var userId = Guid.NewGuid().ToString();
        var userName = "testuser";
        var email = "test@example.com";
        var roles = new List<string> { "User" };

        var token = _tokenGenerator.GenerateToken(userId, userName, email, roles);

        token.Should().NotBeNullOrEmpty();
    }

    /// <summary>
    /// Test weryfikuj¹cy ¿e wygenerowany token zawiera wszystkie wymagane claims.
    /// Sprawdza standardowe claims JWT (Sub, Email, NameIdentifier, Name) oraz role.
    /// </summary>
    /// <remarks>
    /// Dekoduje token u¿ywaj¹c JwtSecurityTokenHandler i analizuje jego zawartoœæ.
    /// Weryfikuje ¿e wszystkie przekazane dane s¹ obecne w tokenie jako claims.
    /// Test wa¿ny dla poprawnoœci autoryzacji i personalizacji.
    /// </remarks>
    [Fact]
    public void GenerateToken_ValidInput_TokenShouldContainClaims()
    {
        var userId = Guid.NewGuid().ToString();
        var userName = "testuser";
        var email = "test@example.com";
        var roles = new List<string> { "User", "Admin" };

        var token = _tokenGenerator.GenerateToken(userId, userName, email, roles);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        jwtToken.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == userId);
        jwtToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Name && c.Value == userName);
        jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == email);
        jwtToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Role && c.Value == "User");
        jwtToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Role && c.Value == "Admin");
    }

    /// <summary>
    /// Test weryfikuj¹cy ¿e dodatkowe niestandardowe claims s¹ poprawnie dodawane do tokenu.
    /// Sprawdza rozszerzalnoœæ systemu o custom claims.
    /// </summary>
    /// <remarks>
    /// U¿ywane do dodawania specyficznych dla aplikacji claims (np. FirstName, LastName, Permissions).
    /// Test wa¿ny dla zaawansowanych scenariuszy autoryzacji.
    /// </remarks>
    [Fact]
    public void GenerateToken_WithAdditionalClaims_ShouldIncludeAllClaims()
    {
        var userId = Guid.NewGuid().ToString();
        var userName = "testuser";
        var email = "test@example.com";
        var roles = new List<string> { "User" };
        var additionalClaims = new List<Claim>
        {
            new Claim("CustomClaim", "CustomValue"),
            new Claim("AnotherClaim", "AnotherValue")
        };

        var token = _tokenGenerator.GenerateToken(userId, userName, email, roles, additionalClaims);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        jwtToken.Claims.Should().Contain(c => c.Type == "CustomClaim" && c.Value == "CustomValue");
        jwtToken.Claims.Should().Contain(c => c.Type == "AnotherClaim" && c.Value == "AnotherValue");
    }

    /// <summary>
    /// Test weryfikuj¹cy ¿e token zawiera poprawne wartoœci Issuer i Audience.
    /// Sprawdza konfiguracjê bezpieczeñstwa tokenu - kto go wyda³ i dla kogo jest przeznaczony.
    /// </summary>
    /// <remarks>
    /// Issuer i Audience s¹ wa¿ne dla walidacji tokenu po stronie serwera.
    /// Zapobiegaj¹ u¿yciu tokenów z innych systemów.
    /// </remarks>
    [Fact]
    public void GenerateToken_ShouldSetCorrectIssuerAndAudience()
    {
        var userId = Guid.NewGuid().ToString();
        var userName = "testuser";
        var email = "test@example.com";
        var roles = new List<string> { "User" };

        var token = _tokenGenerator.GenerateToken(userId, userName, email, roles);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        jwtToken.Issuer.Should().Be("TestIssuer");
        jwtToken.Audiences.Should().Contain("TestAudience");
    }

    /// <summary>
    /// Test weryfikuj¹cy ¿e token ma poprawnie ustawiony czas wygaœniêcia.
    /// Sprawdza czy ExpiryMinutes z konfiguracji jest prawid³owo stosowany.
    /// </summary>
    /// <remarks>
    /// Test z tolerancj¹ czasow¹ (59-61 minut) aby uwzglêdniæ czas wykonania testu.
    /// Wa¿ny dla security - tokeny nie powinny byæ wa¿ne w nieskoñczonoœæ.
    /// </remarks>
    [Fact]
    public void GenerateToken_ShouldSetExpirationTime()
    {
        var userId = Guid.NewGuid().ToString();
        var userName = "testuser";
        var email = "test@example.com";
        var roles = new List<string> { "User" };

        var beforeGeneration = DateTime.UtcNow;
        var token = _tokenGenerator.GenerateToken(userId, userName, email, roles);
        var afterGeneration = DateTime.UtcNow;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        jwtToken.ValidTo.Should().BeAfter(beforeGeneration.AddMinutes(59));
        jwtToken.ValidTo.Should().BeBefore(afterGeneration.AddMinutes(61));
    }

    /// <summary>
    /// Test weryfikuj¹cy ¿e token bez ról nie zawiera ¿adnych claims typu Role.
    /// Sprawdza poprawne dzia³anie przy pustej liœcie ról.
    /// </summary>
    /// <remarks>
    /// Edge case - u¿ytkownik bez przypisanych ról (np. nowo utworzone konto).
    /// </remarks>
    [Fact]
    public void GenerateToken_EmptyRoles_ShouldNotIncludeRoleClaims()
    {
        var userId = Guid.NewGuid().ToString();
        var userName = "testuser";
        var email = "test@example.com";
        var roles = new List<string>();

        var token = _tokenGenerator.GenerateToken(userId, userName, email, roles);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        jwtToken.Claims.Should().NotContain(c => c.Type == ClaimTypes.Role);
    }

    /// <summary>
    /// Test weryfikuj¹cy ¿e generator nie rzuca wyj¹tku gdy lista dodatkowych claims jest null.
    /// Sprawdza obs³ugê opcjonalnego parametru.
    /// </summary>
    /// <remarks>
    /// Test defensive programming - metoda powinna dzia³aæ poprawnie z null claims.
    /// </remarks>
    [Fact]
    public void GenerateToken_NullAdditionalClaims_ShouldNotThrow()
    {
        var userId = Guid.NewGuid().ToString();
        var userName = "testuser";
        var email = "test@example.com";
        var roles = new List<string> { "User" };

        var act = () => _tokenGenerator.GenerateToken(userId, userName, email, roles, null);

        act.Should().NotThrow();
    }
}
