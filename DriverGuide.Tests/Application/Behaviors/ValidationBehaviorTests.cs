using DriverGuide.Application.Behaviors;
using DriverGuide.Application.Requests;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using NSubstitute;

namespace DriverGuide.Tests.Application.Behaviors;

/// <summary>
/// Klasa testowa dla ValidationBehavior - pipeline behavior MediatR.
/// Testuje automatyczn¹ walidacjê ¿¹dañ przed ich przetworzeniem przez handlery.
/// Weryfikuje integracjê FluentValidation z pipeline MediatR.
/// </summary>
/// <remarks>
/// ValidationBehavior jest kluczowym elementem architektury CQRS.
/// Zapewnia ¿e wszystkie ¿¹dania s¹ walidowane przed wykonaniem logiki biznesowej.
/// </remarks>
public class ValidationBehaviorTests
{
    private readonly RequestHandlerDelegate<string> _next;

    /// <summary>
    /// Konstruktor inicjalizuj¹cy mock delegata nastêpnego elementu w pipeline.
    /// Mock symuluje handler zwracaj¹cy "Success" jako wynik operacji.
    /// </summary>
    public ValidationBehaviorTests()
    {
        _next = Substitute.For<RequestHandlerDelegate<string>>();
        _next().Returns(Task.FromResult("Success"));
    }

    /// <summary>
    /// Test weryfikuj¹cy ¿e behavior przekazuje ¿¹danie dalej gdy brak walidatorów.
    /// Sprawdza scenariusz gdzie dla danego typu ¿¹dania nie ma zarejestrowanych walidatorów.
    /// </summary>
    /// <remarks>
    /// Edge case - nie wszystkie ¿¹dania musz¹ mieæ walidatory.
    /// Behavior powinien dzia³aæ transparentnie gdy walidacja nie jest potrzebna.
    /// Test weryfikuje ¿e delegat _next zosta³ wywo³any dok³adnie raz.
    /// </remarks>
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

    /// <summary>
    /// Test weryfikuj¹cy ¿e behavior przekazuje ¿¹danie dalej gdy walidacja przebieg³a pomyœlnie.
    /// Sprawdza scenariusz happy path - ¿¹danie jest poprawne i przechodzi walidacjê.
    /// </summary>
    /// <remarks>
    /// Test g³ównej œcie¿ki dzia³ania behavior.
    /// Mockuje walidator zwracaj¹cy pusty ValidationResult (brak b³êdów).
    /// Weryfikuje ¿e po udanej walidacji handler jest wywo³ywany normalnie.
    /// </remarks>
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

    /// <summary>
    /// Test weryfikuj¹cy ¿e behavior rzuca ValidationException gdy walidacja nie powiedzie siê.
    /// Sprawdza scenariusz negatywny - ¿¹danie zawiera nieprawid³owe dane.
    /// </summary>
    /// <remarks>
    /// Test kluczowy dla security - zapewnia ¿e nieprawid³owe dane nie dotr¹ do handlera.
    /// Mockuje walidator zwracaj¹cy ValidationFailure dla pola Login.
    /// Weryfikuje ¿e:
    /// - ValidationException jest rzucany
    /// - Handler (_next) NIE jest wywo³ywany
    /// Chroni przed przetworzeniem nieprawid³owych ¿¹dañ.
    /// </remarks>
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

    /// <summary>
    /// Test weryfikuj¹cy ¿e behavior agreguje b³êdy z wielu walidatorów.
    /// Sprawdza scenariusz gdzie ¿¹danie jest walidowane przez kilka walidatorów jednoczeœnie.
    /// </summary>
    /// <remarks>
    /// Test zaawansowanego scenariusza - kompozycja walidatorów.
    /// System mo¿e mieæ wiele walidatorów dla jednego typu ¿¹dania (np. podstawowy + biznesowy).
    /// Mockuje dwa walidatory:
    /// - Pierwszy zwraca b³¹d dla Login
    /// - Drugi zwraca b³¹d dla Password
    /// Weryfikuje ¿e ValidationException zawiera oba b³êdy (agregacja).
    /// Wa¿ne dla pe³nego feedbacku u¿ytkownikowi o wszystkich b³êdach walidacji.
    /// </remarks>
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

    /// <summary>
    /// Test weryfikuj¹cy ¿e behavior rzuca wyj¹tek jeœli którykolwiek walidator zawiedzie.
    /// Sprawdza scenariusz mieszany - czêœæ walidatorów przechodzi, czêœæ zawodzi.
    /// </summary>
    /// <remarks>
    /// Test logiki "fail-fast" - wystarczy jeden b³¹d aby odrzuciæ ¿¹danie.
    /// Mockuje dwa walidatory:
    /// - Pierwszy przechodzi pomyœlnie (pusty ValidationResult)
    /// - Drugi zwraca b³¹d dla Password
    /// Weryfikuje ¿e ValidationException jest rzucany mimo czêœciowego sukcesu.
    /// Zapewnia spójnoœæ - wszystkie walidacje musz¹ przejœæ pomyœlnie.
    /// </remarks>
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
