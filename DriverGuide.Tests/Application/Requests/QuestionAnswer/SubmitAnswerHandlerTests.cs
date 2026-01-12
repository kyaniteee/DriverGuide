using DriverGuide.Application.Commands;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using FluentAssertions;
using NSubstitute;

namespace DriverGuide.Tests.Application.Commands.QuestionAnswer;

public class SubmitAnswerHandlerTests
{
    private readonly IQuestionAnswerRepository _questionAnswerRepository;
    private readonly SubmitAnswerHandler _handler;

    public SubmitAnswerHandlerTests()
    {
        _questionAnswerRepository = Substitute.For<IQuestionAnswerRepository>();
        _handler = new SubmitAnswerHandler(_questionAnswerRepository);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldUpdateQuestionAnswer()
    {
        var testSessionId = Guid.NewGuid().ToString();
        var questionId = 123;
        var userAnswer = "A";
        var endDate = DateTimeOffset.Now;

        var command = new SubmitAnswerCommand
        {
            TestSessionId = testSessionId,
            QuestionId = questionId,
            UserAnswer = userAnswer,
            EndDate = endDate
        };

        var existingQuestionAnswer = new DriverGuide.Domain.Models.QuestionAnswer
        {
            QuestionAnswerId = Guid.NewGuid().ToString(),
            TestSessionId = testSessionId,
            QuestionId = questionId,
            UserQuestionAnswer = null,
            EndDate = null
        };

        _questionAnswerRepository.GetAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<DriverGuide.Domain.Models.QuestionAnswer, bool>>>())
            .Returns(Task.FromResult<DriverGuide.Domain.Models.QuestionAnswer?>(existingQuestionAnswer));

        await _handler.Handle(command, CancellationToken.None);

        existingQuestionAnswer.UserQuestionAnswer.Should().Be(userAnswer);
        existingQuestionAnswer.EndDate.Should().Be(endDate);
        await _questionAnswerRepository.Received(1).UpdateAsync(existingQuestionAnswer);
    }

    [Fact]
    public async Task Handle_QuestionAnswerNotFound_ShouldThrowException()
    {
        var command = new SubmitAnswerCommand
        {
            TestSessionId = Guid.NewGuid().ToString(),
            QuestionId = 123,
            UserAnswer = "A"
        };

        _questionAnswerRepository.GetAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<DriverGuide.Domain.Models.QuestionAnswer, bool>>>())
            .Returns(Task.FromResult<DriverGuide.Domain.Models.QuestionAnswer?>(null));

        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_NullEndDate_ShouldUseCurrentTime()
    {
        var testSessionId = Guid.NewGuid().ToString();
        var questionId = 123;

        var command = new SubmitAnswerCommand
        {
            TestSessionId = testSessionId,
            QuestionId = questionId,
            UserAnswer = "A",
            EndDate = null
        };

        var existingQuestionAnswer = new DriverGuide.Domain.Models.QuestionAnswer
        {
            QuestionAnswerId = Guid.NewGuid().ToString(),
            TestSessionId = testSessionId,
            QuestionId = questionId
        };

        _questionAnswerRepository.GetAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<DriverGuide.Domain.Models.QuestionAnswer, bool>>>())
            .Returns(Task.FromResult<DriverGuide.Domain.Models.QuestionAnswer?>(existingQuestionAnswer));

        await _handler.Handle(command, CancellationToken.None);

        existingQuestionAnswer.EndDate.Should().NotBeNull();
        existingQuestionAnswer.EndDate.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromSeconds(5));
    }
}
