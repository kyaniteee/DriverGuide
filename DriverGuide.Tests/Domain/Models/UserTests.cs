using DriverGuide.Domain.Models;
using FluentAssertions;

namespace DriverGuide.Tests.Domain.Models;

public class UserTests
{
    [Fact]
    public void User_ShouldHaveDefaultValues()
    {
        var user = new User();

        user.Id.Should().Be(Guid.Empty);
        user.UserName.Should().BeNull();
        user.Email.Should().BeNull();
        user.Avatar.Should().BeNull();
        user.RefreshToken.Should().BeNull();
    }

    [Fact]
    public void User_ShouldAllowSettingAllProperties()
    {
        var userId = Guid.NewGuid();
        var userName = "testuser";
        var email = "test@example.com";
        var avatar = "avatar.jpg";
        
        var user = new User
        {
            Id = userId,
            UserName = userName,
            Email = email,
            Avatar = avatar
        };

        user.Id.Should().Be(userId);
        user.UserName.Should().Be(userName);
        user.Email.Should().Be(email);
        user.Avatar.Should().Be(avatar);
    }

    [Fact]
    public void User_RefreshToken_ShouldStoreTokenAndExpiry()
    {
        var refreshToken = "test-refresh-token";
        var expiryTime = DateTime.UtcNow.AddDays(7);
        
        var user = new User
        {
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = expiryTime
        };

        user.RefreshToken.Should().Be(refreshToken);
        user.RefreshTokenExpiryTime.Should().Be(expiryTime);
    }

    [Fact]
    public void User_Collections_ShouldBeInitializable()
    {
        var user = new User
        {
            UserRoles = new List<UserRole>(),
            Claims = new List<Microsoft.AspNetCore.Identity.IdentityUserClaim<Guid>>(),
            Logins = new List<Microsoft.AspNetCore.Identity.IdentityUserLogin<Guid>>(),
            Tokens = new List<Microsoft.AspNetCore.Identity.IdentityUserToken<Guid>>()
        };

        user.UserRoles.Should().NotBeNull();
        user.Claims.Should().NotBeNull();
        user.Logins.Should().NotBeNull();
        user.Tokens.Should().NotBeNull();
    }

    [Fact]
    public void User_UserRoles_ShouldAcceptMultipleRoles()
    {
        var user = new User
        {
            UserRoles = new List<UserRole>
            {
                new UserRole { RoleId = Guid.NewGuid() },
                new UserRole { RoleId = Guid.NewGuid() }
            }
        };

        user.UserRoles.Should().HaveCount(2);
    }
}
