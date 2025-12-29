using DriverGuide.Application.Requests;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using FluentAssertions;
using MediatR;
using NSubstitute;

namespace DriverGuide.Tests.Application.Requests.TestSession;

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
    public async Task Handle_ValidRequest_ShouldCompleteTestSession()
    {
        var testSessionId = Guid.NewGuid().ToString();
        var request = new CompleteTestSessionRequest
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

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().Be(Unit.Value);
        existingSession.EndDate.Should().NotBeNull();
        existingSession.Result.Should().Be(85.5);
        await _testSessionRepository.Received(1).UpdateAsync(existingSession);
    }

    [Fact]
    public async Task Handle_TestSessionNotFound_ShouldThrowException()
    {
        var request = new CompleteTestSessionRequest
        {
            TestSessionId = Guid.NewGuid().ToString(),
            Result = 85.5
        };

        _testSessionRepository.GetAsync(Arg.Any<System.Linq.Expressions.Expression<Func<DriverGuide.Domain.Models.TestSession, bool>>>())
            .Returns(Task.FromResult<DriverGuide.Domain.Models.TestSession?>(null));

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ResultIsZero_ShouldCompleteWithZeroResult()
    {
        var testSessionId = Guid.NewGuid().ToString();
        var request = new CompleteTestSessionRequest
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

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().Be(Unit.Value);
        existingSession.Result.Should().Be(0);
    }
}
