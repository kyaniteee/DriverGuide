using DriverGuide.Application.Commands;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using FluentAssertions;
using NSubstitute;

namespace DriverGuide.Tests.Application.Commands.QuestionFile;

public class CreateQuestionFileHandlerTests
{
    private readonly IQuestionFileRepository _questionFileRepository;
    private readonly CreateQuestionFileHandler _handler;

    public CreateQuestionFileHandlerTests()
    {
        _questionFileRepository = Substitute.For<IQuestionFileRepository>();
        _handler = new CreateQuestionFileHandler(_questionFileRepository);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldCreateFileAndReturnGuid()
    {
        var fileBytes = new byte[] { 1, 2, 3, 4, 5 };
        var request = new CreateQuestionFileCommand
        {
            File = fileBytes,
            FileName = "test.jpg"
        };

        _questionFileRepository.CreateAsync(Arg.Any<DriverGuide.Domain.Models.QuestionFile>())
            .Returns(Task.FromResult(new DriverGuide.Domain.Models.QuestionFile()));

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeEmpty();
        await _questionFileRepository.Received(1).CreateAsync(
            Arg.Is<DriverGuide.Domain.Models.QuestionFile>(f =>
                f.Name == request.FileName &&
                f.File == fileBytes));
    }

    [Fact]
    public async Task Handle_Mp4File_ShouldSetCorrectContentType()
    {
        var fileBytes = new byte[] { 1, 2, 3 };
        var request = new CreateQuestionFileCommand
        {
            File = fileBytes,
            FileName = "video.mp4"
        };

        _questionFileRepository.CreateAsync(Arg.Any<DriverGuide.Domain.Models.QuestionFile>())
            .Returns(Task.FromResult(new DriverGuide.Domain.Models.QuestionFile()));

        var result = await _handler.Handle(request, CancellationToken.None);

        await _questionFileRepository.Received(1).CreateAsync(
            Arg.Is<DriverGuide.Domain.Models.QuestionFile>(f =>
                f.ContentType == "video/mp4"));
    }

    [Fact]
    public async Task Handle_PngFile_ShouldSetCorrectContentType()
    {
        var fileBytes = new byte[] { 1, 2, 3 };
        var request = new CreateQuestionFileCommand
        {
            File = fileBytes,
            FileName = "image.png"
        };

        _questionFileRepository.CreateAsync(Arg.Any<DriverGuide.Domain.Models.QuestionFile>())
            .Returns(Task.FromResult(new DriverGuide.Domain.Models.QuestionFile()));

        var result = await _handler.Handle(request, CancellationToken.None);

        await _questionFileRepository.Received(1).CreateAsync(
            Arg.Is<DriverGuide.Domain.Models.QuestionFile>(f =>
                f.ContentType == "image/png"));
    }
}
