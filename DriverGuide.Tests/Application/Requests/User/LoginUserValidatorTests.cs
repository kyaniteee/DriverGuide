using DriverGuide.Application.Requests;
using FluentValidation.TestHelper;

namespace DriverGuide.Tests.Application.Requests.User;

public class LoginUserValidatorTests
{
    private readonly LoginUserValidator _validator;

    public LoginUserValidatorTests()
    {
        _validator = new LoginUserValidator();
    }

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
