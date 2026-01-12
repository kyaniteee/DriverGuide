using DriverGuide.Application.Commands;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using FluentAssertions;
using NSubstitute;

namespace DriverGuide.Tests.Application.Commands.QuestionAnswer;

public class BulkAnswersHandlerTests
{
    private readonly IQuestionAnswerRepository _questionAnswerRepository;
    private readonly BulkAnswersHandler _handler;

    public BulkAnswersHandlerTests()
    {
        _questionAnswerRepository = Substitute.For<IQuestionAnswerRepository>();
        _handler = new BulkAnswersHandler(_questionAnswerRepository);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldUpdateAllAnswers()
    {
        var testSessionId = Guid.NewGuid().ToString();
        var answers = new List<BulkAnswerItem>
        {
            new BulkAnswerItem { QuestionId = 1, UserQuestionAnswer = "A", EndDate = DateTimeOffset.Now },
            new BulkAnswerItem { QuestionId = 2, UserQuestionAnswer = "B", EndDate = DateTimeOffset.Now }
        };

        var command = new BulkAnswersCommand
        {
            TestSessionId = testSessionId,
            Answers = answers
        };

        var questionAnswer1 = new DriverGuide.Domain.Models.QuestionAnswer
        {
            QuestionAnswerId = Guid.NewGuid().ToString(),
            TestSessionId = testSessionId,
            QuestionId = 1
        };

        var questionAnswer2 = new DriverGuide.Domain.Models.QuestionAnswer
        {
            QuestionAnswerId = Guid.NewGuid().ToString(),
            TestSessionId = testSessionId,
            QuestionId = 2
        };

        _questionAnswerRepository.GetAsync(
            Arg.Is<System.Linq.Expressions.Expression<System.Func<DriverGuide.Domain.Models.QuestionAnswer, bool>>>(
                expr => expr.Compile()(questionAnswer1)))
            .Returns(Task.FromResult<DriverGuide.Domain.Models.QuestionAnswer?>(questionAnswer1));

        _questionAnswerRepository.GetAsync(
            Arg.Is<System.Linq.Expressions.Expression<System.Func<DriverGuide.Domain.Models.QuestionAnswer, bool>>>(
                expr => expr.Compile()(questionAnswer2)))
            .Returns(Task.FromResult<DriverGuide.Domain.Models.QuestionAnswer?>(questionAnswer2));

        await _handler.Handle(command, CancellationToken.None);

        questionAnswer1.UserQuestionAnswer.Should().Be("A");
        questionAnswer2.UserQuestionAnswer.Should().Be("B");
        await _questionAnswerRepository.Received(2).UpdateAsync(Arg.Any<DriverGuide.Domain.Models.QuestionAnswer>());
    }

    [Fact]
    public async Task Handle_AnswerNotFound_ShouldSkipThatAnswer()
    {
        var testSessionId = Guid.NewGuid().ToString();
        var answers = new List<BulkAnswerItem>
        {
            new BulkAnswerItem { QuestionId = 1, UserQuestionAnswer = "A", EndDate = DateTimeOffset.Now },
            new BulkAnswerItem { QuestionId = 999, UserQuestionAnswer = "B", EndDate = DateTimeOffset.Now }
        };

        var command = new BulkAnswersCommand
        {
            TestSessionId = testSessionId,
            Answers = answers
        };

        var questionAnswer1 = new DriverGuide.Domain.Models.QuestionAnswer
        {
            QuestionAnswerId = Guid.NewGuid().ToString(),
            TestSessionId = testSessionId,
            QuestionId = 1
        };

        _questionAnswerRepository.GetAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<DriverGuide.Domain.Models.QuestionAnswer, bool>>>())
            .Returns(
                callInfo => Task.FromResult<DriverGuide.Domain.Models.QuestionAnswer?>(questionAnswer1),
                callInfo => Task.FromResult<DriverGuide.Domain.Models.QuestionAnswer?>(null)
            );

        await _handler.Handle(command, CancellationToken.None);

        await _questionAnswerRepository.Received(1).UpdateAsync(Arg.Any<DriverGuide.Domain.Models.QuestionAnswer>());
    }

    [Fact]
    public async Task Handle_EmptyAnswersList_ShouldNotUpdateAnything()
    {
        var command = new BulkAnswersCommand
        {
            TestSessionId = Guid.NewGuid().ToString(),
            Answers = new List<BulkAnswerItem>()
        };

        await _handler.Handle(command, CancellationToken.None);

        await _questionAnswerRepository.DidNotReceive().UpdateAsync(Arg.Any<DriverGuide.Domain.Models.QuestionAnswer>());
    }
}
