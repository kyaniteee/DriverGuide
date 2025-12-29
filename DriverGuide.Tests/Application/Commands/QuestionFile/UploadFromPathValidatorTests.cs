using DriverGuide.Application.Commands;
using FluentValidation.TestHelper;

namespace DriverGuide.Tests.Application.Commands.QuestionFile;

public class UploadFromPathValidatorTests
{
    private readonly UploadFromPathValidator _validator;

    public UploadFromPathValidatorTests()
    {
        _validator = new UploadFromPathValidator();
    }

    [Fact]
    public async Task Validate_EmptyDirectoryPath_ShouldHaveValidationError()
    {
        var request = new UploadFromPathCommand
        {
            DirectoryPath = string.Empty
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.DirectoryPath)
            .WithErrorMessage("Œcie¿ka do katalogu jest wymagana");
    }

    [Fact]
    public async Task Validate_NonExistingDirectory_ShouldHaveValidationError()
    {
        var request = new UploadFromPathCommand
        {
            DirectoryPath = "C:\\NonExistingDirectory\\Path"
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.DirectoryPath)
            .WithErrorMessage("Podany katalog nie istnieje");
    }
}
