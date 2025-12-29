using FluentValidation;

namespace DriverGuide.Application.Commands;

public class UploadFromPathValidator : AbstractValidator<UploadFromPathCommand>
{
    public UploadFromPathValidator()
    {
        RuleFor(x => x.DirectoryPath)
            .NotEmpty().WithMessage("Œcie¿ka do katalogu jest wymagana")
            .Must(Directory.Exists).WithMessage("Podany katalog nie istnieje");
    }
}
