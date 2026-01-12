using DriverGuide.Application.Commands;
using FluentValidation.TestHelper;

namespace DriverGuide.Tests.Application.Commands.QuestionAnswer;

public class SubmitAnswerValidatorTests
{
    private readonly SubmitAnswerValidator _validator;

    public SubmitAnswerValidatorTests()
    {
        _validator = new SubmitAnswerValidator();
    }

    [Fact]
    public async Task Validate_ValidCommand_ShouldNotHaveValidationError()
    {
        var command = new SubmitAnswerCommand
        {
            TestSessionId = Guid.NewGuid().ToString(),
            QuestionId = 123,
            UserAnswer = "A"
        };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_EmptyTestSessionId_ShouldHaveValidationError()
    {
        var command = new SubmitAnswerCommand
        {
            TestSessionId = string.Empty,
            QuestionId = 123,
            UserAnswer = "A"
        };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.TestSessionId)
            .WithErrorMessage("TestSessionId jest wymagane");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Validate_InvalidQuestionId_ShouldHaveValidationError(int questionId)
    {
        var command = new SubmitAnswerCommand
        {
            TestSessionId = Guid.NewGuid().ToString(),
            QuestionId = questionId,
            UserAnswer = "A"
        };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.QuestionId)
            .WithErrorMessage("QuestionId jest wymagana");
    }

    [Fact]
    public async Task Validate_EmptyUserAnswer_ShouldHaveValidationError()
    {
        var command = new SubmitAnswerCommand
        {
            TestSessionId = Guid.NewGuid().ToString(),
            QuestionId = 123,
            UserAnswer = string.Empty
        };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.UserAnswer)
            .WithErrorMessage("Odpowiedü uøytkownika jest wymagana");
    }
}
