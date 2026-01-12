using DriverGuide.Application.Commands;
using DriverGuide.Domain.Enums;
using FluentValidation.TestHelper;

namespace DriverGuide.Tests.Application.Commands.QuestionAnswer;

public class StartQuestionValidatorTests
{
    private readonly StartQuestionValidator _validator;

    public StartQuestionValidatorTests()
    {
        _validator = new StartQuestionValidator();
    }

    [Fact]
    public async Task Validate_ValidCommand_ShouldNotHaveValidationError()
    {
        var command = new StartQuestionCommand
        {
            TestSessionId = Guid.NewGuid().ToString(),
            QuestionId = 123,
            QuestionCategory = LicenseCategory.B,
            Question = "Test question?",
            CorrectQuestionAnswer = "A",
            StartDate = DateTimeOffset.Now,
            QuestionLanguage = Language.PL
        };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_EmptyTestSessionId_ShouldHaveValidationError()
    {
        var command = new StartQuestionCommand
        {
            TestSessionId = string.Empty,
            QuestionId = 123,
            QuestionCategory = LicenseCategory.B,
            Question = "Test question?",
            StartDate = DateTimeOffset.Now
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
        var command = new StartQuestionCommand
        {
            TestSessionId = Guid.NewGuid().ToString(),
            QuestionId = questionId,
            QuestionCategory = LicenseCategory.B,
            Question = "Test question?",
            StartDate = DateTimeOffset.Now
        };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.QuestionId)
            .WithErrorMessage("QuestionId musi byæ wiêksze od 0");
    }

    [Fact]
    public async Task Validate_EmptyQuestion_ShouldHaveValidationError()
    {
        var command = new StartQuestionCommand
        {
            TestSessionId = Guid.NewGuid().ToString(),
            QuestionId = 123,
            QuestionCategory = LicenseCategory.B,
            Question = string.Empty,
            StartDate = DateTimeOffset.Now
        };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Question)
            .WithErrorMessage("Treœæ pytania jest wymagana");
    }
}
