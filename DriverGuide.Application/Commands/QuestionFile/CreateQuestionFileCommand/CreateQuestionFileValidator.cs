using FluentValidation;

namespace DriverGuide.Application.Commands;

public class CreateQuestionFileValidator : AbstractValidator<CreateQuestionFileCommand>
{
    public CreateQuestionFileValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("Nieistniejący plik!");

        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("Nazwa pliku jest pusta!")
            .When(x => x.FileName?.Split('.').Count() < 2).WithMessage("Nazwa pliku nie zawiera rozszerzenia!");
    }
}
