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
        // Example using ASP.NET Core Identity's PasswordHasher
        var passwordHasher = new PasswordHasher<User>();
        var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, password);
        return await Task.FromResult(verificationResult == PasswordVerificationResult.Success);
    }

    public async Task<bool> VerifyTwoFactorCodeAsync(User user, string twoFactorCode)
    {
        // Replace with your actual 2FA provider logic (e.g., using Microsoft.AspNetCore.Identity.UserManager)
        // This is a placeholder for demonstration purposes.
        if (string.IsNullOrWhiteSpace(twoFactorCode))
            return false;

        // Example: Use a TOTP provider or similar
        // bool isValid = await _twoFactorProvider.ValidateAsync(user, twoFactorCode);
        // return isValid;

        // For now, just return false to indicate not implemented
        return false;
    }

    public async Task<bool> VerifyTwoFactorRecoveryCodeAsync(User user, string recoveryCode)
    {
        if (user == null || string.IsNullOrWhiteSpace(recoveryCode))
            return false;

        // Example: assuming user.Tokens contains recovery codes as tokens (adapt as needed)
        var normalizedCode = recoveryCode.Trim().Replace(" ", string.Empty);

        var recoveryToken = user.Tokens?
            .FirstOrDefault(t => t.LoginProvider == "RecoveryCode" && t.Value == normalizedCode);

        if (recoveryToken != null)
        {
            // Optionally: remove the used recovery code to prevent reuse
            user.Tokens.Remove(recoveryToken);
            // Save changes to DB if using EF Core, e.g.:
            // await _dbContext.SaveChangesAsync();
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
