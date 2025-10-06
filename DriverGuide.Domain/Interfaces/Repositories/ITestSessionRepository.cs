using DriverGuide.Domain.Models;
using DriverGuide.Domain.Enums;

namespace DriverGuide.Domain.Interfaces;

public interface ITestSessionRepository : IRepositoryBase<TestSession>
{
    Task<TestSession> GetByIdAsync(string testSessionId);
    Task<TestSession> CreateSessionAsync(LicenseCategory category, Guid? userId = null);
    Task<bool> CompleteSessionAsync(string testSessionId, double result);
}