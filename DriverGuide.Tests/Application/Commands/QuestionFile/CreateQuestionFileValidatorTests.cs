using DriverGuide.Application.Commands;
using FluentValidation.TestHelper;

namespace DriverGuide.Tests.Application.Commands.QuestionFile;

public class CreateQuestionFileValidatorTests
{
    private readonly CreateQuestionFileValidator _validator;

    public CreateQuestionFileValidatorTests()
    {
        _validator = new CreateQuestionFileValidator();
    }

    [Fact]
    public async Task Validate_ValidRequest_ShouldNotHaveValidationError()
    {
        var request = new CreateQuestionFileCommand
        {
            File = new byte[] { 1, 2, 3, 4, 5 },
            FileName = "test.jpg"
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_NullFile_ShouldHaveValidationError()
    {
        var request = new CreateQuestionFileCommand
        {
            File = null,
            FileName = "test.jpg"
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.File)
            .WithErrorMessage("Nieistniej¹cy plik!");
    }

    [Fact]
    public async Task Validate_EmptyFileName_ShouldHaveValidationError()
    {
        var request = new CreateQuestionFileCommand
        {
            File = new byte[] { 1, 2, 3 },
            FileName = string.Empty
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.FileName)
            .WithErrorMessage("Nazwa pliku jest pusta!");
    }

    [Theory]
    [InlineData("test.jpg")]
    [InlineData("video.mp4")]
    [InlineData("image.png")]
    [InlineData("document.pdf")]
    public async Task Validate_ValidFileExtensions_ShouldNotHaveValidationError(string fileName)
    {
        var request = new CreateQuestionFileCommand
        {
            File = new byte[] { 1, 2, 3 },
            FileName = fileName
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_EmptyByteArray_ShouldBeAllowed()
    {
        var request = new CreateQuestionFileCommand
        {
            File = Array.Empty<byte>(),
            FileName = "empty.txt"
        };

        var result = await _validator.TestValidateAsync(request);

        result.ShouldNotHaveValidationErrorFor(x => x.File);
    }
}
