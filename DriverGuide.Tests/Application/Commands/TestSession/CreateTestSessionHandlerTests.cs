using DriverGuide.Application.Commands;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using FluentAssertions;
using NSubstitute;

namespace DriverGuide.Tests.Application.Commands.TestSession;

public class CreateTestSessionHandlerTests
{
    private readonly ITestSessionRepository _testSessionRepository;
    private readonly IUserRepository _userRepository;
    private readonly CreateTestSessionHandler _handler;

    public CreateTestSessionHandlerTests()
    {
        _testSessionRepository = Substitute.For<ITestSessionRepository>();
        _userRepository = Substitute.For<IUserRepository>();
        _handler = new CreateTestSessionHandler(_testSessionRepository, _userRepository);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldCreateTestSessionAndReturnGuid()
    {
        var userId = Guid.NewGuid();
        var request = new CreateTestSessionCommand
        {
            UserId = userId.ToString()
        };

        var user = new DriverGuide.Domain.Models.User
        {
            Id = userId,
            UserName = "testuser",
            Email = "test@example.com"
        };

        _userRepository.GetByGuidAsync(userId)
            .Returns(Task.FromResult<DriverGuide.Domain.Models.User?>(user));

        _testSessionRepository.CreateAsync(Arg.Any<DriverGuide.Domain.Models.TestSession>())
            .Returns(Task.FromResult(new DriverGuide.Domain.Models.TestSession()));

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeEmpty();
        await _testSessionRepository.Received(1).CreateAsync(Arg.Any<DriverGuide.Domain.Models.TestSession>());
    }

    [Fact]
    public async Task Handle_UserNotFound_ShouldThrowException()
    {
        var userId = Guid.NewGuid();
        var request = new CreateTestSessionCommand
        {
            UserId = userId.ToString()
        };

        _userRepository.GetByGuidAsync(userId)
            .Returns(Task.FromResult<DriverGuide.Domain.Models.User?>(null));

        await Assert.ThrowsAsync<Exception>(
            () => _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_NullUserId_ShouldCreateAnonymousTestSession()
    {
        var request = new CreateTestSessionCommand
        {
            UserId = null
        };

        _testSessionRepository.CreateAsync(Arg.Any<DriverGuide.Domain.Models.TestSession>())
            .Returns(Task.FromResult(new DriverGuide.Domain.Models.TestSession()));

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeEmpty();
        await _testSessionRepository.Received(1).CreateAsync(Arg.Is<DriverGuide.Domain.Models.TestSession>(
            ts => ts.UserId == null));
        await _userRepository.DidNotReceive().GetByGuidAsync(Arg.Any<Guid>());
    }

    [Fact]
    public async Task Handle_EmptyUserId_ShouldCreateAnonymousTestSession()
    {
        var request = new CreateTestSessionCommand
        {
            UserId = string.Empty
        };

        _testSessionRepository.CreateAsync(Arg.Any<DriverGuide.Domain.Models.TestSession>())
            .Returns(Task.FromResult(new DriverGuide.Domain.Models.TestSession()));

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeEmpty();
        await _testSessionRepository.Received(1).CreateAsync(Arg.Is<DriverGuide.Domain.Models.TestSession>(
            ts => ts.UserId == null));
        await _userRepository.DidNotReceive().GetByGuidAsync(Arg.Any<Guid>());
    }
}
