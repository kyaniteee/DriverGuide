using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DriverGuide.Infrastructure.Database;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(DriverGuideDbContext context) : base(context) { }

    public async Task<bool> VerifyPasswordAsync(User user, string password)
    {
        var passwordHasher = new PasswordHasher<User>();
        var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, password);
        return await Task.FromResult(verificationResult == PasswordVerificationResult.Success);
    }

    public async Task<bool> VerifyTwoFactorCodeAsync(User user, string twoFactorCode)
    {
        if (string.IsNullOrWhiteSpace(twoFactorCode))
            return false;
        return false;
    }

    public async Task<bool> VerifyTwoFactorRecoveryCodeAsync(User user, string recoveryCode)
    {
        if (user == null || string.IsNullOrWhiteSpace(recoveryCode))
            return false;

        var normalizedCode = recoveryCode.Trim().Replace(" ", string.Empty);

        var recoveryToken = user.Tokens?
            .FirstOrDefault(t => t.LoginProvider == "RecoveryCode" && t.Value == normalizedCode);

        if (recoveryToken != null)
        {
            user.Tokens.Remove(recoveryToken);
            return true;
        }

        return false;
    }

    public async Task<User?> GetWithRolesAndClaimsAsync(string loginOrEmail)
    {
        return await ((DriverGuideDbContext)Context).Users
            .Include(u => u.UserRoles!)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.Claims)
            .FirstOrDefaultAsync(u => u.UserName == loginOrEmail || u.Email == loginOrEmail);
    }
}
