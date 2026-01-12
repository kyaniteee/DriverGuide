using DriverGuide.Application.Commands;
using DriverGuide.Domain.Models;
using FluentValidation.TestHelper;

namespace DriverGuide.Tests.Application.Commands.QuestionAnswer;

public class BulkAnswersValidatorTests
{
    private readonly BulkAnswersValidator _validator;

    public BulkAnswersValidatorTests()
    {
        _validator = new BulkAnswersValidator();
    }

    [Fact]
    public async Task Validate_ValidCommand_ShouldNotHaveValidationError()
    {
        var command = new BulkAnswersCommand
        {
            TestSessionId = Guid.NewGuid().ToString(),
            Answers = new List<BulkAnswerItem>
            {
                new BulkAnswerItem { QuestionId = 1, UserQuestionAnswer = "A" },
                new BulkAnswerItem { QuestionId = 2, UserQuestionAnswer = "B" }
            }
        };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_EmptyTestSessionId_ShouldHaveValidationError()
    {
        var command = new BulkAnswersCommand
        {
            TestSessionId = string.Empty,
            Answers = new List<BulkAnswerItem>
            {
                new BulkAnswerItem { QuestionId = 1, UserQuestionAnswer = "A" }
            }
        };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.TestSessionId)
            .WithErrorMessage("TestSessionId jest wymagane");
    }

    [Fact]
    public async Task Validate_EmptyAnswers_ShouldHaveValidationError()
    {
        var command = new BulkAnswersCommand
        {
            TestSessionId = Guid.NewGuid().ToString(),
            Answers = new List<BulkAnswerItem>()
        };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Answers)
            .WithErrorMessage("Lista odpowiedzi nie mo¿e byæ pusta");
    }

    [Fact]
    public async Task Validate_InvalidQuestionId_ShouldHaveValidationError()
    {
        var command = new BulkAnswersCommand
        {
            TestSessionId = Guid.NewGuid().ToString(),
            Answers = new List<BulkAnswerItem>
            {
                new BulkAnswerItem { QuestionId = 0, UserQuestionAnswer = "A" }
            }
        };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor("Answers[0].QuestionId")
            .WithErrorMessage("QuestionId musi byæ wiêksze od 0");
    }

    [Fact]
    public async Task Validate_EmptyUserAnswer_ShouldHaveValidationError()
    {
        var command = new BulkAnswersCommand
        {
            TestSessionId = Guid.NewGuid().ToString(),
            Answers = new List<BulkAnswerItem>
            {
                new BulkAnswerItem { QuestionId = 1, UserQuestionAnswer = string.Empty }
            }
        };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor("Answers[0].UserQuestionAnswer")
            .WithErrorMessage("OdpowiedŸ u¿ytkownika jest wymagana");
    }
}
