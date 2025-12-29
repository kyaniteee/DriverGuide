using DriverGuide.Application.Requests;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using FluentAssertions;
using MediatR;
using NSubstitute;

namespace DriverGuide.Tests.Application.Requests.QuestionAnswer;

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
    public async Task Handle_ValidRequest_ShouldUpdateQuestionAnswer()
    {
        var testSessionId = Guid.NewGuid().ToString();
        var questionId = "123";
        var request = new SubmitAnswerRequest
        {
            TestSessionId = testSessionId,
            QuestionId = questionId,
            UserAnswer = "A",
            EndDate = DateTimeOffset.Now
        };

        var existingAnswer = new DriverGuide.Domain.Models.QuestionAnswer
        {
            QuestionAnswerId = Guid.NewGuid().ToString(),
            TestSessionId = testSessionId,
            QuestionId = questionId,
            UserQuestionAnswer = null,
            EndDate = null
        };

        _questionAnswerRepository.GetAsync(Arg.Any<System.Linq.Expressions.Expression<Func<DriverGuide.Domain.Models.QuestionAnswer, bool>>>())
            .Returns(Task.FromResult<DriverGuide.Domain.Models.QuestionAnswer?>(existingAnswer));

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().Be(Unit.Value);
        existingAnswer.UserQuestionAnswer.Should().Be("A");
        existingAnswer.EndDate.Should().NotBeNull();
        await _questionAnswerRepository.Received(1).UpdateAsync(existingAnswer);
    }

    [Fact]
    public async Task Handle_QuestionAnswerNotFound_ShouldThrowException()
    {
        var request = new SubmitAnswerRequest
        {
            TestSessionId = Guid.NewGuid().ToString(),
            QuestionId = "123",
            UserAnswer = "A"
        };

        _questionAnswerRepository.GetAsync(Arg.Any<System.Linq.Expressions.Expression<Func<DriverGuide.Domain.Models.QuestionAnswer, bool>>>())
            .Returns(Task.FromResult<DriverGuide.Domain.Models.QuestionAnswer?>(null));

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_NullEndDate_ShouldUseCurrentTime()
    {
        var testSessionId = Guid.NewGuid().ToString();
        var questionId = "123";
        var request = new SubmitAnswerRequest
        {
            TestSessionId = testSessionId,
            QuestionId = questionId,
            UserAnswer = "B",
            EndDate = null
        };

        var existingAnswer = new DriverGuide.Domain.Models.QuestionAnswer
        {
            QuestionAnswerId = Guid.NewGuid().ToString(),
            TestSessionId = testSessionId,
            QuestionId = questionId,
            UserQuestionAnswer = null,
            EndDate = null
        };

        _questionAnswerRepository.GetAsync(Arg.Any<System.Linq.Expressions.Expression<Func<DriverGuide.Domain.Models.QuestionAnswer, bool>>>())
            .Returns(Task.FromResult<DriverGuide.Domain.Models.QuestionAnswer?>(existingAnswer));

        var beforeCall = DateTimeOffset.Now;
        var result = await _handler.Handle(request, CancellationToken.None);
        var afterCall = DateTimeOffset.Now;

        result.Should().Be(Unit.Value);
        existingAnswer.EndDate.Should().NotBeNull();
        existingAnswer.EndDate.Should().BeOnOrAfter(beforeCall);
        existingAnswer.EndDate.Should().BeOnOrBefore(afterCall);
    }
}
