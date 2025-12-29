using DriverGuide.Application.Requests;
using DriverGuide.Application.Services;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using FluentAssertions;
using NSubstitute;

namespace DriverGuide.Tests.Application.Requests.User;

public class LoginUserHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly LoginUserHandler _handler;

    public LoginUserHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        _handler = new LoginUserHandler(_userRepository, _jwtTokenGenerator);
    }

    [Fact]
    public async Task Handle_ValidCredentials_ShouldReturnToken()
    {
        var request = new LoginUserRequest
        {
            Login = "testuser",
            Password = "Password123!"
        };

        var user = new DriverGuide.Domain.Models.User
        {
            Id = Guid.NewGuid(),
            UserName = "testuser",
            Email = "test@example.com",
            UserRoles = new List<UserRole>()
        };

        var expectedToken = "generated-jwt-token";

        _userRepository.GetWithRolesAndClaimsAsync(request.Login)
            .Returns(Task.FromResult<DriverGuide.Domain.Models.User?>(user));

        _userRepository.VerifyPasswordAsync(user, request.Password)
            .Returns(Task.FromResult(true));

        _jwtTokenGenerator.GenerateToken(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<List<string>>(),
            Arg.Any<List<System.Security.Claims.Claim>>())
            .Returns(expectedToken);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().Be(expectedToken);
        await _userRepository.Received(1).GetWithRolesAndClaimsAsync(request.Login);
        await _userRepository.Received(1).VerifyPasswordAsync(user, request.Password);
    }

    [Fact]
    public async Task Handle_UserNotFound_ShouldThrowUnauthorizedException()
    {
        var request = new LoginUserRequest
        {
            Login = "nonexistent",
            Password = "Password123!"
        };

        _userRepository.GetWithRolesAndClaimsAsync(request.Login)
            .Returns(Task.FromResult<DriverGuide.Domain.Models.User?>(null));

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_InvalidPassword_ShouldThrowUnauthorizedException()
    {
        var request = new LoginUserRequest
        {
            Login = "testuser",
            Password = "WrongPassword!"
        };

        var user = new DriverGuide.Domain.Models.User
        {
            Id = Guid.NewGuid(),
            UserName = "testuser",
            Email = "test@example.com"
        };

        _userRepository.GetWithRolesAndClaimsAsync(request.Login)
            .Returns(Task.FromResult<DriverGuide.Domain.Models.User?>(user));

        _userRepository.VerifyPasswordAsync(user, request.Password)
            .Returns(Task.FromResult(false));

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _handler.Handle(request, CancellationToken.None));
    }
}
