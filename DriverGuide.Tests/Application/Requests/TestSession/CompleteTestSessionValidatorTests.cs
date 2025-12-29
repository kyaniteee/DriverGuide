using DriverGuide.Application.Requests;
using FluentValidation.TestHelper;

namespace DriverGuide.Tests.Application.Requests.TestSession;

public class CompleteTestSessionValidatorTests
{
    private readonly CompleteTestSessionValidator _validator;

    public CompleteTestSessionValidatorTests()
    {
        _validator = new CompleteTestSessionValidator();
    }

    [Fact]
    public async Task Validate_ValidRequest_ShouldNotHaveValidationError()
    {
        var request = new CompleteTestSessionRequest
        {
            TestSessionId = Guid.NewGuid().ToString(),
            Result = 75.5
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_EmptyTestSessionId_ShouldHaveValidationError()
    {
        var request = new CompleteTestSessionRequest
        {
            TestSessionId = string.Empty,
            Result = 75.5
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.TestSessionId)
            .WithErrorMessage("TestSessionId jest wymagane");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-50)]
    public async Task Validate_NegativeResult_ShouldHaveValidationError(double resultValue)
    {
        var request = new CompleteTestSessionRequest
        {
            TestSessionId = Guid.NewGuid().ToString(),
            Result = resultValue
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Result)
            .WithErrorMessage("Wynik nie mo¿e byæ ujemny");
    }

    [Theory]
    [InlineData(101)]
    [InlineData(150)]
    public async Task Validate_ResultAbove100_ShouldHaveValidationError(double resultValue)
    {
        var request = new CompleteTestSessionRequest
        {
            TestSessionId = Guid.NewGuid().ToString(),
            Result = resultValue
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Result)
            .WithErrorMessage("Wynik nie mo¿e przekraczaæ 100%");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100)]
    [InlineData(50.5)]
    public async Task Validate_ValidResultBoundaries_ShouldNotHaveValidationError(double resultValue)
    {
        var request = new CompleteTestSessionRequest
        {
            TestSessionId = Guid.NewGuid().ToString(),
            Result = resultValue
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldNotHaveValidationErrorFor(x => x.Result);
    }
}
