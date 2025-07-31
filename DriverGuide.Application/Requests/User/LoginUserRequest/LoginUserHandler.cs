using DriverGuide.Domain.Interfaces;
using MediatR;

namespace DriverGuide.Application.Requests;

public class LoginUserHandler(IUserRepository userRepository) : IRequestHandler<LoginUserRequest, Guid>
{
    public async Task<Guid> Handle(LoginUserRequest request, CancellationToken cancellationToken)
    {
        // Find user by login or email
        var user = await userRepository.GetAsync(x => x.UserName == request.Login || x.UserName == request.Login, useNoTracking: true) ?? throw new UnauthorizedAccessException("Invalid login or email.");

        // Check password (assuming userRepository has a method for password verification)
        var passwordValid = await userRepository.VerifyPasswordAsync(user, request.Password);
        if (!passwordValid)
            throw new UnauthorizedAccessException("Invalid password.");

        // Optionally handle two-factor authentication
        if (!string.IsNullOrEmpty(user.TwoFactorEnabled.ToString()) && user.TwoFactorEnabled == true)
        {
            bool twoFactorValid = false;
            //if (!string.IsNullOrEmpty(request.TwoFactorCode))
            //{
            //    twoFactorValid = await userRepository.VerifyTwoFactorCodeAsync(user, request.TwoFactorCode);
            //}
            //else if (!string.IsNullOrEmpty(request.TwoFactorRecoveryCode))
            //{
            //    twoFactorValid = await userRepository.VerifyTwoFactorRecoveryCodeAsync(user, request.TwoFactorRecoveryCode);
            //}

            if (!twoFactorValid)
                throw new UnauthorizedAccessException("Invalid two-factor authentication code.");
        }

        return user.Id;
    }
}