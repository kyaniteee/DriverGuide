using DriverGuide.Application.Requests;
using DriverGuide.Domain.Enums;
using DriverGuide.Domain.Models;
using FluentValidation.TestHelper;

namespace DriverGuide.Tests.Application.Requests.QuestionAnswer;

public class BulkAnswersValidatorTests
{
    private readonly BulkAnswersValidator _validator;

    public BulkAnswersValidatorTests()
    {
        _validator = new BulkAnswersValidator();
    }

    [Fact]
    public async Task Validate_ValidRequest_ShouldNotHaveValidationError()
    {
        var request = new BulkAnswersRequest
        {
            TestSessionId = Guid.NewGuid().ToString(),
            Answers = new List<BulkAnswerItem>
            {
                new BulkAnswerItem
                {
                    QuestionId = "1",
                    UserQuestionAnswer = "A",
                    EndDate = DateTimeOffset.Now
                }
            }
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_EmptyTestSessionId_ShouldHaveValidationError()
    {
        var request = new BulkAnswersRequest
        {
            TestSessionId = string.Empty,
            Answers = new List<BulkAnswerItem>()
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.TestSessionId)
            .WithErrorMessage("TestSessionId jest wymagane");
    }

    [Fact]
    public async Task Validate_NullAnswers_ShouldHaveValidationError()
    {
        var request = new BulkAnswersRequest
        {
            TestSessionId = Guid.NewGuid().ToString(),
            Answers = null!
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Answers)
            .WithErrorMessage("Lista odpowiedzi jest wymagana");
    }

    [Fact]
    public async Task Validate_EmptyAnswers_ShouldHaveValidationError()
    {
        var request = new BulkAnswersRequest
        {
            TestSessionId = Guid.NewGuid().ToString(),
            Answers = new List<BulkAnswerItem>()
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Answers)
            .WithErrorMessage("Lista odpowiedzi nie mo¿e byæ pusta");
    }

    [Fact]
    public async Task Validate_AnswerWithEmptyQuestionId_ShouldHaveValidationError()
    {
        var request = new BulkAnswersRequest
        {
            TestSessionId = Guid.NewGuid().ToString(),
            Answers = new List<BulkAnswerItem>
            {
                new BulkAnswerItem
                {
                    QuestionId = string.Empty,
                    UserQuestionAnswer = "A"
                }
            }
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor("Answers[0].QuestionId");
    }

    [Fact]
    public async Task Validate_AnswerWithEmptyUserAnswer_ShouldHaveValidationError()
    {
        var request = new BulkAnswersRequest
        {
            TestSessionId = Guid.NewGuid().ToString(),
            Answers = new List<BulkAnswerItem>
            {
                new BulkAnswerItem
                {
                    QuestionId = "1",
                    UserQuestionAnswer = string.Empty
                }
            }
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor("Answers[0].UserQuestionAnswer");
    }
}
