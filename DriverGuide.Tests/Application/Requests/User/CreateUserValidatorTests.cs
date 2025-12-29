using DriverGuide.Application.Requests;
using FluentValidation.TestHelper;
using NSubstitute;

namespace DriverGuide.Tests.Application.Requests.User;

public class CreateUserValidatorTests
{
    private readonly CreateUserValidator _validator;
    private readonly DriverGuide.Domain.Interfaces.IUserRepository _userRepository;

    public CreateUserValidatorTests()
    {
        _userRepository = Substitute.For<DriverGuide.Domain.Interfaces.IUserRepository>();
        _validator = new CreateUserValidator(_userRepository);
    }

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
