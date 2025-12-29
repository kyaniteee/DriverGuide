using DriverGuide.Application.Requests;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using FluentAssertions;
using NSubstitute;

namespace DriverGuide.Tests.Application.Requests.User;

public class CreateUserHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly CreateUserHandler _handler;

    public CreateUserHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _handler = new CreateUserHandler(_userRepository);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldCreateUserAndReturnGuid()
    {
        var request = new CreateUserRequest
        {
            Login = "testuser",
            FirstName = "Test",
            LastName = "User",
            BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-20)),
            Email = "test@example.com",
            Password = "Password123!"
        };

        var userId = Guid.NewGuid();
        var createdUser = new DriverGuide.Domain.Models.User
        {
            Id = userId,
            UserName = request.Login,
            Email = request.Email
        };

        _userRepository.CreateAsync(Arg.Any<DriverGuide.Domain.Models.User>())
            .Returns(Task.FromResult(createdUser));

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().Be(userId);
        await _userRepository.Received(1).CreateAsync(Arg.Any<DriverGuide.Domain.Models.User>());
    }

    [Fact]
    public async Task Handle_CreateUserFails_ShouldThrowException()
    {
        var request = new CreateUserRequest
        {
            Login = "testuser",
            FirstName = "Test",
            LastName = "User",
            BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-20)),
            Email = "test@example.com",
            Password = "Password123!"
        };

        _userRepository.CreateAsync(Arg.Any<DriverGuide.Domain.Models.User>())
            .Returns(Task.FromException<DriverGuide.Domain.Models.User>(new Exception("Database error")));

        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
    }
}
