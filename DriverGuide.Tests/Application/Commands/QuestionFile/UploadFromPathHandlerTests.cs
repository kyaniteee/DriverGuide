using DriverGuide.Application.Commands;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using FluentAssertions;
using NSubstitute;

namespace DriverGuide.Tests.Application.Commands.QuestionFile;

public class UploadFromPathHandlerTests
{
    private readonly IQuestionFileRepository _questionFileRepository;
    private readonly UploadFromPathHandler _handler;

    public UploadFromPathHandlerTests()
    {
        _questionFileRepository = Substitute.For<IQuestionFileRepository>();
        _handler = new UploadFromPathHandler(_questionFileRepository);
    }

    [Fact]
    public async Task Handle_NullDirectoryPath_ShouldThrowArgumentNullException()
    {
        var request = new UploadFromPathCommand
        {
            DirectoryPath = null
        };

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_EmptyDirectoryPath_ShouldThrowArgumentNullException()
    {
        var request = new UploadFromPathCommand
        {
            DirectoryPath = string.Empty
        };

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhitespaceDirectoryPath_ShouldThrowArgumentNullException()
    {
        var request = new UploadFromPathCommand
        {
            DirectoryPath = "   "
        };

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _handler.Handle(request, CancellationToken.None));
    }
}
