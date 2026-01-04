using DriverGuide.Application.Requests;
using FluentValidation.TestHelper;

namespace DriverGuide.Tests.Application.Requests.QuestionAnswer;

public class SubmitAnswerValidatorTests
{
    private readonly SubmitAnswerValidator _validator;

    public SubmitAnswerValidatorTests()
    {
        _validator = new SubmitAnswerValidator();
    }

    [Fact]
    public async Task Validate_ValidRequest_ShouldNotHaveValidationError()
    {
        var request = new SubmitAnswerRequest
        {
            TestSessionId = Guid.NewGuid().ToString(),
            QuestionId = 123,
            UserAnswer = "A"
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_EmptyTestSessionId_ShouldHaveValidationError()
    {
        var request = new SubmitAnswerRequest
        {
            TestSessionId = string.Empty,
            QuestionId = 123,
            UserAnswer = "A"
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.TestSessionId)
            .WithErrorMessage("TestSessionId jest wymagane");
    }

    [Fact]
    public async Task Validate_EmptyQuestionId_ShouldHaveValidationError()
    {
        var request = new SubmitAnswerRequest
        {
            TestSessionId = Guid.NewGuid().ToString(),
            QuestionId = 0,
            UserAnswer = "A"
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.QuestionId)
            .WithErrorMessage("QuestionId jest wymagane");
    }

    [Fact]
    public async Task Validate_EmptyUserAnswer_ShouldHaveValidationError()
    {
        var request = new SubmitAnswerRequest
        {
            TestSessionId = Guid.NewGuid().ToString(),
            QuestionId = 123,
            UserAnswer = string.Empty
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.UserAnswer)
            .WithErrorMessage("Odpowiedü uøytkownika jest wymagana");
    }
}
