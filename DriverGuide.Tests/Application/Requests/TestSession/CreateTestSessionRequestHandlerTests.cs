using DriverGuide.Application.Requests;
using DriverGuide.Domain.Enums;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using FluentAssertions;
using NSubstitute;

namespace DriverGuide.Tests.Application.Requests.TestSession;

public class CreateTestSessionRequestHandlerTests
{
    private readonly ITestSessionRepository _testSessionRepository;
    private readonly CreateTestSessionRequestHandler _handler;

    public CreateTestSessionRequestHandlerTests()
    {
        _testSessionRepository = Substitute.For<ITestSessionRepository>();
        _handler = new CreateTestSessionRequestHandler(_testSessionRepository);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldCreateTestSessionAndReturnGuid()
    {
        var userId = Guid.NewGuid();
        var request = new CreateTestSessionRequest
        {
            StartDate = DateTimeOffset.Now,
            Category = LicenseCategory.B,
            UserId = userId
        };

        _testSessionRepository.CreateAsync(Arg.Any<DriverGuide.Domain.Models.TestSession>())
            .Returns(Task.FromResult(new DriverGuide.Domain.Models.TestSession()));

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeEmpty();
        await _testSessionRepository.Received(1).CreateAsync(
            Arg.Is<DriverGuide.Domain.Models.TestSession>(s =>
                s.StartDate == request.StartDate &&
                s.UserId == userId));
    }

    [Fact]
    public async Task Handle_AnonymousUser_ShouldCreateSessionWithNullUserId()
    {
        var request = new CreateTestSessionRequest
        {
            StartDate = DateTimeOffset.Now,
            Category = LicenseCategory.A,
            UserId = null
        };

        _testSessionRepository.CreateAsync(Arg.Any<DriverGuide.Domain.Models.TestSession>())
            .Returns(Task.FromResult(new DriverGuide.Domain.Models.TestSession()));

        var result = await _handler.Handle(request, CancellationToken.None);

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
        var request = new CreateTestSessionRequest
        {
            StartDate = DateTimeOffset.Now,
            Category = category,
            UserId = Guid.NewGuid()
        };

        _testSessionRepository.CreateAsync(Arg.Any<DriverGuide.Domain.Models.TestSession>())
            .Returns(Task.FromResult(new DriverGuide.Domain.Models.TestSession()));

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeEmpty();
    }
}
