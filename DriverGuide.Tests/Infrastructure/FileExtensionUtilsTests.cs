using DriverGuide.Domain.Enums;
using DriverGuide.Domain.Models;
using DriverGuide.Infrastructure;
using FluentAssertions;

namespace DriverGuide.Tests.Infrastructure;

public class FileExtensionUtilsTests
{
    [Theory]
    [InlineData("test.jpg", "image/jpeg")]
    [InlineData("test.jpeg", "image/jpeg")]
    [InlineData("test.png", "image/png")]
    [InlineData("test.gif", "image/gif")]
    [InlineData("test.bmp", "image/bmp")]
    [InlineData("test.webp", "image/webp")]
    [InlineData("test.mp4", "video/mp4")]
    [InlineData("test.avi", "video/x-msvideo")]
    [InlineData("test.mov", "video/quicktime")]
    [InlineData("test.wmv", "video/x-ms-wmv")]
    [InlineData("test.webm", "video/webm")]
    [InlineData("test.pdf", "application/pdf")]
    public void GetFileMimeType_ValidExtensions_ShouldReturnCorrectMimeType(string fileName, string expectedMimeType)
    {
        var result = DGEnvironment.GetFileMimeType(fileName);

        result.Should().Be(expectedMimeType);
    }

    [Theory]
    [InlineData("test.unknown")]
    [InlineData("test")]
    [InlineData("")]
    public void GetFileMimeType_UnknownExtension_ShouldReturnOctetStream(string fileName)
    {
        var result = DGEnvironment.GetFileMimeType(fileName);

        result.Should().Be("application/octet-stream");
    }

    [Theory]
    [InlineData("TEST.JPG", "image/jpeg")]
    [InlineData("TEST.MP4", "video/mp4")]
    [InlineData("TeSt.PnG", "image/png")]
    public void GetFileMimeType_CaseInsensitive_ShouldWork(string fileName, string expectedMimeType)
    {
        var result = DGEnvironment.GetFileMimeType(fileName);

        result.Should().Be(expectedMimeType);
    }

    [Fact]
    public void GetFileMimeType_NullFileName_ShouldReturnOctetStream()
    {
        var result = DGEnvironment.GetFileMimeType(null!);

        result.Should().Be("application/octet-stream");
    }
}
