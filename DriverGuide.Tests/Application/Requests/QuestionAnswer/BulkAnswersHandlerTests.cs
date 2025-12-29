using DriverGuide.Application.Requests;
using DriverGuide.Domain.Enums;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using FluentAssertions;
using MediatR;
using NSubstitute;

namespace DriverGuide.Tests.Application.Requests.QuestionAnswer;

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
    public async Task Handle_ValidRequest_ShouldUpdateAllAnswers()
    {
        var testSessionId = Guid.NewGuid().ToString();
        var answers = new List<BulkAnswerItem>
        {
            new BulkAnswerItem
            {
                QuestionId = "1",
                UserQuestionAnswer = "A",
                EndDate = DateTimeOffset.Now
            },
            new BulkAnswerItem
            {
                QuestionId = "2",
                UserQuestionAnswer = "B",
                EndDate = DateTimeOffset.Now
            }
        };

        var request = new BulkAnswersRequest
        {
            TestSessionId = testSessionId,
            Answers = answers
        };

        var existingAnswer1 = new DriverGuide.Domain.Models.QuestionAnswer
        {
            QuestionAnswerId = Guid.NewGuid().ToString(),
            TestSessionId = testSessionId,
            QuestionId = "1"
        };

        var existingAnswer2 = new DriverGuide.Domain.Models.QuestionAnswer
        {
            QuestionAnswerId = Guid.NewGuid().ToString(),
            TestSessionId = testSessionId,
            QuestionId = "2"
        };

        _questionAnswerRepository.GetAsync(Arg.Any<System.Linq.Expressions.Expression<Func<DriverGuide.Domain.Models.QuestionAnswer, bool>>>())
            .Returns(Task.FromResult<DriverGuide.Domain.Models.QuestionAnswer?>(existingAnswer1),
                     Task.FromResult<DriverGuide.Domain.Models.QuestionAnswer?>(existingAnswer2));

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().Be(Unit.Value);
        await _questionAnswerRepository.Received(2).UpdateAsync(Arg.Any<DriverGuide.Domain.Models.QuestionAnswer>());
    }

    [Fact]
    public async Task Handle_AnswerNotFound_ShouldSkipUpdate()
    {
        var testSessionId = Guid.NewGuid().ToString();
        var answers = new List<BulkAnswerItem>
        {
            new BulkAnswerItem
            {
                QuestionId = "999",
                UserQuestionAnswer = "A",
                EndDate = DateTimeOffset.Now
            }
        };

        var request = new BulkAnswersRequest
        {
            TestSessionId = testSessionId,
            Answers = answers
        };

        _questionAnswerRepository.GetAsync(Arg.Any<System.Linq.Expressions.Expression<Func<DriverGuide.Domain.Models.QuestionAnswer, bool>>>())
            .Returns(Task.FromResult<DriverGuide.Domain.Models.QuestionAnswer?>(null));

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().Be(Unit.Value);
        await _questionAnswerRepository.DidNotReceive().UpdateAsync(Arg.Any<DriverGuide.Domain.Models.QuestionAnswer>());
    }

    [Fact]
    public async Task Handle_EmptyAnswersList_ShouldCompleteSuccessfully()
    {
        var request = new BulkAnswersRequest
        {
            TestSessionId = Guid.NewGuid().ToString(),
            Answers = new List<BulkAnswerItem>()
        };

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().Be(Unit.Value);
        await _questionAnswerRepository.DidNotReceive().UpdateAsync(Arg.Any<DriverGuide.Domain.Models.QuestionAnswer>());
    }
}
