using DriverGuide.Application.Commands;
using DriverGuide.Domain.Enums;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using FluentAssertions;
using NSubstitute;

namespace DriverGuide.Tests.Application.Commands.QuestionAnswer;

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
    public async Task Handle_ValidCommand_ShouldCreateQuestionAnswerAndReturnGuid()
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

        _questionAnswerRepository.CreateAsync(Arg.Any<DriverGuide.Domain.Models.QuestionAnswer>())
            .Returns(Task.FromResult(new DriverGuide.Domain.Models.QuestionAnswer()));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeEmpty();
        await _questionAnswerRepository.Received(1).CreateAsync(
            Arg.Is<DriverGuide.Domain.Models.QuestionAnswer>(qa => 
                qa.TestSessionId == command.TestSessionId &&
                qa.QuestionId == command.QuestionId &&
                qa.QuestionCategory == command.QuestionCategory &&
                qa.QuestionText == command.Question &&
                qa.CorrectQuestionAnswer == command.CorrectQuestionAnswer &&
                qa.QuestionLanguage == command.QuestionLanguage));
    }

    [Fact]
    public async Task Handle_NullCorrectAnswer_ShouldCreateWithNullCorrectAnswer()
    {
        var command = new StartQuestionCommand
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

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeEmpty();
        await _questionAnswerRepository.Received(1).CreateAsync(
            Arg.Is<DriverGuide.Domain.Models.QuestionAnswer>(qa => qa.CorrectQuestionAnswer == null));
    }
}
