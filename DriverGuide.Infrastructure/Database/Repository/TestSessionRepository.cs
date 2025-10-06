using DriverGuide.Domain.Enums;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DriverGuide.Infrastructure.Database;

public class TestSessionRepository : RepositoryBase<TestSession>, ITestSessionRepository
{
    public TestSessionRepository(DriverGuideDbContext context) : base(context) { }

    public async Task<TestSession> GetByIdAsync(string testSessionId)
    {
        return await Context.TestSessions!
            .FirstOrDefaultAsync(ts => ts.TestSessionId == testSessionId);
    }

    public async Task<TestSession> CreateSessionAsync(LicenseCategory category, Guid? userId = null)
    {
        var session = new TestSession
        {
            TestSessionId = Guid.NewGuid().ToString(),
            StartDate = DateTimeOffset.Now,
            UserId = userId
        };

        Context.TestSessions!.Add(session);
        await Context.SaveChangesAsync();

        return session;
    }

    public async Task<bool> CompleteSessionAsync(string testSessionId, double result)
    {
        var session = await GetByIdAsync(testSessionId);
        if (session == null)
            return false;

        session.EndDate = DateTimeOffset.Now;
        session.Result = result;

        await Context.SaveChangesAsync();
        return true;
    }
}