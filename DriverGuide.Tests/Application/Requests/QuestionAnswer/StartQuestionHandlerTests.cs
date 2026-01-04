using DriverGuide.Application.Requests;
using DriverGuide.Domain.Enums;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using FluentAssertions;
using NSubstitute;

namespace DriverGuide.Tests.Application.Requests.QuestionAnswer;

public class StartQuestionHandlerTests
{
    private readonly IQuestionAnswerRepository _questionAnswerRepository;
    private readonly StartQuestionHandler _handler;

    public StartQuestionHandlerTests()
    {
        _questionAnswerRepository = Substitute.For<IQuestionAnswerRepository>();
        _handler = new StartQuestionHandler(_questionAnswerRepository);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldCreateQuestionAnswerAndReturnGuid()
    {
        var request = new StartQuestionRequest
        {
            TestSessionId = Guid.NewGuid().ToString(),
            QuestionId = 123,
            QuestionCategory = LicenseCategory.B,
            Question = "Test question?",
            CorrectQuestionAnswer = "A",
            StartDate = DateTimeOffset.Now,
            QuestionLanguage = Language.PL
        };

        _questionAnswerRepository.CreateAsync(Arg.Any<DriverGuide.Domain.Models.QuestionAnswer>())
            .Returns(Task.FromResult(new DriverGuide.Domain.Models.QuestionAnswer()));

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeEmpty();
        await _questionAnswerRepository.Received(1).CreateAsync(
            Arg.Is<DriverGuide.Domain.Models.QuestionAnswer>(qa => 
                qa.TestSessionId == request.TestSessionId &&
                qa.QuestionId == request.QuestionId &&
                qa.QuestionCategory == request.QuestionCategory &&
                qa.QuestionText == request.Question &&
                qa.CorrectQuestionAnswer == request.CorrectQuestionAnswer &&
                qa.QuestionLanguage == request.QuestionLanguage));
    }

    [Fact]
    public async Task Handle_NullCorrectAnswer_ShouldCreateWithNullCorrectAnswer()
    {
        var request = new StartQuestionRequest
        {
            TestSessionId = Guid.NewGuid().ToString(),
            QuestionId = 123,
            QuestionCategory = LicenseCategory.B,
            Question = "Test question?",
            CorrectQuestionAnswer = null,
            StartDate = DateTimeOffset.Now,
            QuestionLanguage = Language.PL
        };

        _questionAnswerRepository.CreateAsync(Arg.Any<DriverGuide.Domain.Models.QuestionAnswer>())
            .Returns(Task.FromResult(new DriverGuide.Domain.Models.QuestionAnswer()));

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeEmpty();
        await _questionAnswerRepository.Received(1).CreateAsync(
            Arg.Is<DriverGuide.Domain.Models.QuestionAnswer>(qa => qa.CorrectQuestionAnswer == null));
    }
}
