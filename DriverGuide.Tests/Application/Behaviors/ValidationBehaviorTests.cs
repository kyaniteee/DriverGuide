using DriverGuide.Application.Behaviors;
using DriverGuide.Application.Requests;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using NSubstitute;

namespace DriverGuide.Tests.Application.Behaviors;

public class ValidationBehaviorTests
{
    private readonly RequestHandlerDelegate<string> _next;

    public ValidationBehaviorTests()
    {
        _next = Substitute.For<RequestHandlerDelegate<string>>();
        _next().Returns(Task.FromResult("Success"));
    }

    [Fact]
    public async Task Handle_NoValidators_ShouldCallNext()
    {
        var validators = Enumerable.Empty<IValidator<LoginUserRequest>>();
        var behavior = new ValidationBehavior<LoginUserRequest, string>(validators);
        var request = new LoginUserRequest { Login = "test", Password = "password" };

        var result = await behavior.Handle(request, _next, CancellationToken.None);

        result.Should().Be("Success");
        await _next.Received(1)();
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldCallNext()
    {
        var validator = Substitute.For<IValidator<LoginUserRequest>>();
        validator.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new ValidationResult()));

        var validators = new[] { validator };
        var behavior = new ValidationBehavior<LoginUserRequest, string>(validators);
        var request = new LoginUserRequest { Login = "test", Password = "password" };

        var result = await behavior.Handle(request, _next, CancellationToken.None);

        result.Should().Be("Success");
        await _next.Received(1)();
    }

    [Fact]
    public async Task Handle_InvalidRequest_ShouldThrowValidationException()
    {
        var validator = Substitute.For<IValidator<LoginUserRequest>>();
        var validationFailure = new ValidationFailure("Login", "Login jest wymagany");
        validator.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new ValidationResult(new[] { validationFailure })));

        var validators = new[] { validator };
        var behavior = new ValidationBehavior<LoginUserRequest, string>(validators);
        var request = new LoginUserRequest { Login = "", Password = "password" };

        await Assert.ThrowsAsync<ValidationException>(
            () => behavior.Handle(request, _next, CancellationToken.None));

        await _next.DidNotReceive()();
    }

    [Fact]
    public async Task Handle_MultipleValidatorsWithFailures_ShouldAggregateErrors()
    {
        var validator1 = Substitute.For<IValidator<LoginUserRequest>>();
        var validationFailure1 = new ValidationFailure("Login", "Login jest wymagany");
        validator1.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new ValidationResult(new[] { validationFailure1 })));

        var validator2 = Substitute.For<IValidator<LoginUserRequest>>();
        var validationFailure2 = new ValidationFailure("Password", "Has³o jest wymagane");
        validator2.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new ValidationResult(new[] { validationFailure2 })));

        var validators = new[] { validator1, validator2 };
        var behavior = new ValidationBehavior<LoginUserRequest, string>(validators);
        var request = new LoginUserRequest { Login = "", Password = "" };

        var exception = await Assert.ThrowsAsync<ValidationException>(
            () => behavior.Handle(request, _next, CancellationToken.None));

        exception.Errors.Should().HaveCount(2);
        exception.Errors.Should().Contain(e => e.PropertyName == "Login");
        exception.Errors.Should().Contain(e => e.PropertyName == "Password");
    }

    [Fact]
    public async Task Handle_SomeValidatorsPass_ShouldStillThrowIfAnyFails()
    {
        var validatorPass = Substitute.For<IValidator<LoginUserRequest>>();
        validatorPass.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new ValidationResult()));

        var validatorFail = Substitute.For<IValidator<LoginUserRequest>>();
        var validationFailure = new ValidationFailure("Password", "Has³o jest za krótkie");
        validatorFail.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new ValidationResult(new[] { validationFailure })));

        var validators = new[] { validatorPass, validatorFail };
        var behavior = new ValidationBehavior<LoginUserRequest, string>(validators);
        var request = new LoginUserRequest { Login = "validuser", Password = "short" };

        await Assert.ThrowsAsync<ValidationException>(
            () => behavior.Handle(request, _next, CancellationToken.None));

        await _next.DidNotReceive()();
    }
}
