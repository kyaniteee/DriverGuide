using DriverGuide.Domain.Models;

namespace DriverGuide.Domain.Interfaces;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<bool> VerifyPasswordAsync(User user, string password);
    Task<bool> VerifyTwoFactorCodeAsync(User user, string twoFactorCode);
    Task<bool> VerifyTwoFactorRecoveryCodeAsync(User user, string recoveryCode);
    Task<User?> GetWithRolesAndClaimsAsync(string loginOrEmail);
}
