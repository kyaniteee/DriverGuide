using DriverGuide.Application.Commands;
using DriverGuide.Domain.Enums;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using FluentAssertions;
using NSubstitute;

namespace DriverGuide.Tests.Application.Commands.TestSession;

public class CreateTestSessionHandlerTests
{
    private readonly ITestSessionRepository _testSessionRepository;
    private readonly CreateTestSessionHandler _handler;

    public CreateTestSessionHandlerTests()
    {
        _testSessionRepository = Substitute.For<ITestSessionRepository>();
        _handler = new CreateTestSessionHandler(_testSessionRepository);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateTestSessionAndReturnGuid()
    {
        var userId = Guid.NewGuid();
        var command = new CreateTestSessionCommand
        {
            StartDate = DateTimeOffset.Now,
            Category = LicenseCategory.B,
            UserId = userId
        };

        _testSessionRepository.CreateAsync(Arg.Any<DriverGuide.Domain.Models.TestSession>())
            .Returns(Task.FromResult(new DriverGuide.Domain.Models.TestSession()));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeEmpty();
        await _testSessionRepository.Received(1).CreateAsync(
            Arg.Is<DriverGuide.Domain.Models.TestSession>(s =>
                s.StartDate == command.StartDate &&
                s.UserId == userId));
    }

    [Fact]
    public async Task Handle_AnonymousUser_ShouldCreateSessionWithNullUserId()
    {
        var command = new CreateTestSessionCommand
        {
            StartDate = DateTimeOffset.Now,
            Category = LicenseCategory.A,
            UserId = null
        };

        _testSessionRepository.CreateAsync(Arg.Any<DriverGuide.Domain.Models.TestSession>())
            .Returns(Task.FromResult(new DriverGuide.Domain.Models.TestSession()));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeEmpty();
        await _testSessionRepository.Received(1).CreateAsync(
            Arg.Is<DriverGuide.Domain.Models.TestSession>(s => s.UserId == null));
    }

    [Theory]
    [InlineData(LicenseCategory.B)]
    [InlineData(LicenseCategory.C)]
    [InlineData(LicenseCategory.D)]
    public async Task Handle_DifferentCategories_ShouldCreateSessionForEachCategory(LicenseCategory category)
    {
        var command = new CreateTestSessionCommand
        {
            StartDate = DateTimeOffset.Now,
            Category = category,
            UserId = Guid.NewGuid()
        };

        _testSessionRepository.CreateAsync(Arg.Any<DriverGuide.Domain.Models.TestSession>())
            .Returns(Task.FromResult(new DriverGuide.Domain.Models.TestSession()));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeEmpty();
    }
}
