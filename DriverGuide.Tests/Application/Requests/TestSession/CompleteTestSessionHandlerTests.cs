using DriverGuide.Application.Commands;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using FluentAssertions;
using NSubstitute;

namespace DriverGuide.Tests.Application.Commands.TestSession;

public class CompleteTestSessionHandlerTests
{
    private readonly ITestSessionRepository _testSessionRepository;
    private readonly CompleteTestSessionHandler _handler;

    public CompleteTestSessionHandlerTests()
    {
        _testSessionRepository = Substitute.For<ITestSessionRepository>();
        _handler = new CompleteTestSessionHandler(_testSessionRepository);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCompleteTestSession()
    {
        var testSessionId = Guid.NewGuid().ToString();
        var command = new CompleteTestSessionCommand
        {
            TestSessionId = testSessionId,
            Result = 85.5
        };

        var existingSession = new DriverGuide.Domain.Models.TestSession
        {
            TestSessionId = testSessionId,
            StartDate = DateTimeOffset.Now.AddHours(-1),
            EndDate = null,
            Result = null
        };

        _testSessionRepository.GetAsync(Arg.Any<System.Linq.Expressions.Expression<Func<DriverGuide.Domain.Models.TestSession, bool>>>())
            .Returns(Task.FromResult<DriverGuide.Domain.Models.TestSession?>(existingSession));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeTrue();
        existingSession.EndDate.Should().NotBeNull();
        existingSession.Result.Should().Be(85.5);
        await _testSessionRepository.Received(1).UpdateAsync(existingSession);
    }

    [Fact]
    public async Task Handle_TestSessionNotFound_ShouldReturnFalse()
    {
        var command = new CompleteTestSessionCommand
        {
            TestSessionId = Guid.NewGuid().ToString(),
            Result = 85.5
        };

        _testSessionRepository.GetAsync(Arg.Any<System.Linq.Expressions.Expression<Func<DriverGuide.Domain.Models.TestSession, bool>>>())
            .Returns(Task.FromResult<DriverGuide.Domain.Models.TestSession?>(null));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeFalse();
        await _testSessionRepository.DidNotReceive().UpdateAsync(Arg.Any<DriverGuide.Domain.Models.TestSession>());
    }

    [Fact]
    public async Task Handle_ResultIsZero_ShouldCompleteWithZeroResult()
    {
        var testSessionId = Guid.NewGuid().ToString();
        var command = new CompleteTestSessionCommand
        {
            TestSessionId = testSessionId,
            Result = 0
        };

        var existingSession = new DriverGuide.Domain.Models.TestSession
        {
            TestSessionId = testSessionId,
            StartDate = DateTimeOffset.Now.AddHours(-1),
            EndDate = null,
            Result = null
        };

        _testSessionRepository.GetAsync(Arg.Any<System.Linq.Expressions.Expression<Func<DriverGuide.Domain.Models.TestSession, bool>>>())
            .Returns(Task.FromResult<DriverGuide.Domain.Models.TestSession?>(existingSession));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeTrue();
        existingSession.Result.Should().Be(0);
    }
}
