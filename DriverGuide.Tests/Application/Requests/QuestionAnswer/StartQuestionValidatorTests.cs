using DriverGuide.Application.Requests;
using DriverGuide.Domain.Enums;
using FluentValidation.TestHelper;

namespace DriverGuide.Tests.Application.Requests.QuestionAnswer;

public class StartQuestionValidatorTests
{
    private readonly StartQuestionValidator _validator;

    public StartQuestionValidatorTests()
    {
        _validator = new StartQuestionValidator();
    }

    [Fact]
    public async Task Validate_ValidRequest_ShouldNotHaveValidationError()
    {
        var request = new StartQuestionRequest
        {
            TestSessionId = Guid.NewGuid().ToString(),
            QuestionId = "123",
            Question = "Test question?",
            QuestionCategory = LicenseCategory.B,
            QuestionLanguage = Language.PL,
            StartDate = DateTimeOffset.Now
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_EmptyTestSessionId_ShouldHaveValidationError()
    {
        var request = new StartQuestionRequest
        {
            TestSessionId = string.Empty,
            QuestionId = "123",
            Question = "Test question?",
            QuestionCategory = LicenseCategory.B,
            StartDate = DateTimeOffset.Now
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.TestSessionId)
            .WithErrorMessage("TestSessionId jest wymagane");
    }

    [Fact]
    public async Task Validate_EmptyQuestionId_ShouldHaveValidationError()
    {
        var request = new StartQuestionRequest
        {
            TestSessionId = Guid.NewGuid().ToString(),
            QuestionId = string.Empty,
            Question = "Test question?",
            QuestionCategory = LicenseCategory.B,
            StartDate = DateTimeOffset.Now
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.QuestionId)
            .WithErrorMessage("QuestionId jest wymagane");
    }

    [Fact]
    public async Task Validate_EmptyQuestion_ShouldHaveValidationError()
    {
        var request = new StartQuestionRequest
        {
            TestSessionId = Guid.NewGuid().ToString(),
            QuestionId = "123",
            Question = string.Empty,
            QuestionCategory = LicenseCategory.B,
            StartDate = DateTimeOffset.Now
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Question)
            .WithErrorMessage("Treœæ pytania jest wymagana");
    }

    [Fact]
    public async Task Validate_FutureStartDate_ShouldHaveValidationError()
    {
        var request = new StartQuestionRequest
        {
            TestSessionId = Guid.NewGuid().ToString(),
            QuestionId = "123",
            Question = "Test question?",
            QuestionCategory = LicenseCategory.B,
            StartDate = DateTimeOffset.Now.AddHours(2)
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.StartDate)
            .WithErrorMessage("Data rozpoczêcia nie mo¿e byæ w przysz³oœci");
    }
}
