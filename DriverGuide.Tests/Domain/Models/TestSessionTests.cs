using DriverGuide.Domain.Models;
using FluentAssertions;

namespace DriverGuide.Tests.Domain.Models;

public class TestSessionTests
{
    [Fact]
    public void TestSession_ShouldHaveDefaultValues()
    {
        var testSession = new TestSession();

        testSession.TestSessionId.Should().BeNull();
        testSession.UserId.Should().BeNull();
        testSession.EndDate.Should().BeNull();
        testSession.Result.Should().BeNull();
    }

    [Fact]
    public void TestSession_ShouldAllowSettingAllProperties()
    {
        var startDate = DateTimeOffset.Now;
        var endDate = DateTimeOffset.Now.AddHours(1);
        var userId = Guid.NewGuid();
        var sessionId = Guid.NewGuid().ToString();
        
        var testSession = new TestSession
        {
            TestSessionId = sessionId,
            UserId = userId,
            StartDate = startDate,
            EndDate = endDate,
            Result = 85.5
        };

        testSession.TestSessionId.Should().Be(sessionId);
        testSession.UserId.Should().Be(userId);
        testSession.StartDate.Should().Be(startDate);
        testSession.EndDate.Should().Be(endDate);
        testSession.Result.Should().Be(85.5);
    }

    [Fact]
    public void TestSession_Anonymous_ShouldHaveNullUserId()
    {
        var testSession = new TestSession
        {
            TestSessionId = Guid.NewGuid().ToString(),
            UserId = null,
            StartDate = DateTimeOffset.Now
        };

        testSession.UserId.Should().BeNull();
    }

    [Fact]
    public void TestSession_InProgress_ShouldHaveNullEndDateAndResult()
    {
        var testSession = new TestSession
        {
            TestSessionId = Guid.NewGuid().ToString(),
            StartDate = DateTimeOffset.Now,
            EndDate = null,
            Result = null
        };

        testSession.EndDate.Should().BeNull();
        testSession.Result.Should().BeNull();
    }

    [Fact]
    public void TestSession_Completed_ShouldHaveEndDateAndResult()
    {
        var testSession = new TestSession
        {
            TestSessionId = Guid.NewGuid().ToString(),
            StartDate = DateTimeOffset.Now.AddHours(-1),
            EndDate = DateTimeOffset.Now,
            Result = 92.3
        };

        testSession.EndDate.Should().NotBeNull();
        testSession.Result.Should().NotBeNull();
        testSession.Result.Should().BeGreaterThanOrEqualTo(0);
        testSession.Result.Should().BeLessThanOrEqualTo(100);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(50)]
    [InlineData(100)]
    public void TestSession_Result_ShouldAcceptValidPercentages(double result)
    {
        var testSession = new TestSession
        {
            Result = result
        };

        testSession.Result.Should().Be(result);
    }
}
