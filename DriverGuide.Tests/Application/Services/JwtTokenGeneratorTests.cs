using DriverGuide.Application.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DriverGuide.Tests.Application.Services;

public class JwtTokenGeneratorTests
{
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IConfiguration _configuration;

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
