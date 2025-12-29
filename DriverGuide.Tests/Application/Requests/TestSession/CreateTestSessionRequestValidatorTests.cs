using DriverGuide.Application.Requests;
using DriverGuide.Domain.Enums;
using FluentValidation.TestHelper;

namespace DriverGuide.Tests.Application.Requests.TestSession;

public class CreateTestSessionRequestValidatorTests
{
    private readonly CreateTestSessionRequestValidator _validator;

    public CreateTestSessionRequestValidatorTests()
    {
        _validator = new CreateTestSessionRequestValidator();
    }

    [Fact]
    public async Task Validate_ValidRequest_ShouldNotHaveValidationError()
    {
        var request = new CreateTestSessionRequest
        {
            StartDate = DateTimeOffset.Now,
            Category = LicenseCategory.B,
            UserId = Guid.NewGuid()
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_FutureStartDate_ShouldHaveValidationError()
    {
        var request = new CreateTestSessionRequest
        {
            StartDate = DateTimeOffset.Now.AddHours(2),
            Category = LicenseCategory.B,
            UserId = Guid.NewGuid()
        };

        var result = await _validator.TestValidateAsync(request);

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
        var request = new CreateTestSessionRequest
        {
            StartDate = DateTimeOffset.Now,
            Category = category,
            UserId = Guid.NewGuid()
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldNotHaveValidationErrorFor(x => x.Category);
    }

    [Fact]
    public async Task Validate_NullUserId_ShouldBeValid()
    {
        var request = new CreateTestSessionRequest
        {
            StartDate = DateTimeOffset.Now,
            Category = LicenseCategory.B,
            UserId = null
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
