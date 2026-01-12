using DriverGuide.Application.Commands;
using FluentValidation.TestHelper;

namespace DriverGuide.Tests.Application.Commands.TestSession;

public class CompleteTestSessionValidatorTests
{
    private readonly CompleteTestSessionValidator _validator;

    public CompleteTestSessionValidatorTests()
    {
        _validator = new CompleteTestSessionValidator();
    }

    [Fact]
    public async Task Validate_ValidCommand_ShouldNotHaveValidationError()
    {
        var command = new CompleteTestSessionCommand
        {
            TestSessionId = Guid.NewGuid().ToString(),
            Result = 75.5
        };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_EmptyTestSessionId_ShouldHaveValidationError()
    {
        var command = new CompleteTestSessionCommand
        {
            TestSessionId = string.Empty,
            Result = 75.5
        };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.TestSessionId)
            .WithErrorMessage("ID sesji testowej jest wymagane");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-50)]
    public async Task Validate_NegativeResult_ShouldHaveValidationError(double resultValue)
    {
        var command = new CompleteTestSessionCommand
        {
            TestSessionId = Guid.NewGuid().ToString(),
            Result = resultValue
        };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Result)
            .WithErrorMessage("Wynik nie mo¿e byæ ujemny");
    }

    [Theory]
    [InlineData(101)]
    [InlineData(150)]
    public async Task Validate_ResultAbove100_ShouldHaveValidationError(double resultValue)
    {
        var command = new CompleteTestSessionCommand
        {
            TestSessionId = Guid.NewGuid().ToString(),
            Result = resultValue
        };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Result)
            .WithErrorMessage("Wynik nie mo¿e przekraczaæ 100");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100)]
    [InlineData(50.5)]
    public async Task Validate_ValidResultBoundaries_ShouldNotHaveValidationError(double resultValue)
    {
        var command = new CompleteTestSessionCommand
        {
            TestSessionId = Guid.NewGuid().ToString(),
            Result = resultValue
        };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Result);
    }
}
