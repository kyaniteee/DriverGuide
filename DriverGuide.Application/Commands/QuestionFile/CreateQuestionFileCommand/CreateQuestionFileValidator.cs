using FluentValidation;

namespace DriverGuide.Application.Commands;

public class CreateQuestionFileValidator : AbstractValidator<CreateQuestionFileCommand>
{
    public CreateQuestionFileValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("Nieistniejący plik!");

        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("Nazwa pliku jest pusta!");

        RuleFor(x => x.FileName)
            .Must(HasFileExtension).WithMessage("Nazwa pliku nie zawiera rozszerzenia!")
            .When(x => !string.IsNullOrEmpty(x.FileName));
    }

    private bool HasFileExtension(string? fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            return false;

        return fileName.Contains('.') && fileName.Split('.').Length >= 2;
    }
}
