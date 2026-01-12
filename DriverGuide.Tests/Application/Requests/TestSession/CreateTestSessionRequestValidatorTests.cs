using DriverGuide.Application.Commands;
using DriverGuide.Domain.Enums;
using FluentValidation.TestHelper;

namespace DriverGuide.Tests.Application.Commands.TestSession;

public class CreateTestSessionValidatorTests
{
    private readonly CreateTestSessionValidator _validator;

    public CreateTestSessionValidatorTests()
    {
        _validator = new CreateTestSessionValidator();
    }

    [Fact]
    public async Task Validate_ValidCommand_ShouldNotHaveValidationError()
    {
        var command = new CreateTestSessionCommand
        {
            StartDate = DateTimeOffset.Now,
            Category = LicenseCategory.B,
            UserId = Guid.NewGuid()
        };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_FutureStartDate_ShouldHaveValidationError()
    {
        var command = new CreateTestSessionCommand
        {
            StartDate = DateTimeOffset.Now.AddHours(2),
            Category = LicenseCategory.B,
            UserId = Guid.NewGuid()
        };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.StartDate)
            .WithErrorMessage("Data rozpoczêcia nie mo¿e byæ w przysz³oœci");
    }

    [Theory]
    [InlineData(LicenseCategory.AM)]
    [InlineData(LicenseCategory.B)]
    [InlineData(LicenseCategory.C)]
    [InlineData(LicenseCategory.D)]
    public async Task Validate_AllCategories_ShouldBeValid(LicenseCategory category)
    {
        var command = new CreateTestSessionCommand
        {
            StartDate = DateTimeOffset.Now,
            Category = category,
            UserId = Guid.NewGuid()
        };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Category);
    }

    [Fact]
    public async Task Validate_NullUserId_ShouldBeValid()
    {
        var command = new CreateTestSessionCommand
        {
            StartDate = DateTimeOffset.Now,
            Category = LicenseCategory.B,
            UserId = null
        };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
