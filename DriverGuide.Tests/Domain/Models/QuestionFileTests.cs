using DriverGuide.Domain.Models;
using FluentAssertions;

namespace DriverGuide.Tests.Domain.Models;

public class QuestionFileTests
{
    [Fact]
    public void QuestionFile_ShouldHaveDefaultValues()
    {
        var questionFile = new QuestionFile();

        questionFile.QuestionFileId.Should().BeNull();
        questionFile.Name.Should().BeNull();
        questionFile.File.Should().BeNull();
        questionFile.ContentType.Should().BeNull();
    }

    [Fact]
    public void QuestionFile_ShouldAllowSettingAllProperties()
    {
        var fileId = Guid.NewGuid().ToString();
        var fileName = "test.jpg";
        var fileBytes = new byte[] { 1, 2, 3, 4, 5 };
        var contentType = "image/jpeg";
        var uploadDate = DateOnly.FromDateTime(DateTime.Now);
        
        var questionFile = new QuestionFile
        {
            QuestionFileId = fileId,
            Name = fileName,
            File = fileBytes,
            ContentType = contentType,
            UploadDate = uploadDate
        };

        questionFile.QuestionFileId.Should().Be(fileId);
        questionFile.Name.Should().Be(fileName);
        questionFile.File.Should().BeEquivalentTo(fileBytes);
        questionFile.ContentType.Should().Be(contentType);
        questionFile.UploadDate.Should().Be(uploadDate);
    }

    [Theory]
    [InlineData("image.jpg", "image/jpeg")]
    [InlineData("video.mp4", "video/mp4")]
    [InlineData("image.png", "image/png")]
    public void QuestionFile_ShouldStoreCorrectContentType(string fileName, string expectedContentType)
    {
        var questionFile = new QuestionFile
        {
            Name = fileName,
            ContentType = expectedContentType
        };

        questionFile.ContentType.Should().Be(expectedContentType);
    }

    [Fact]
    public void QuestionFile_File_ShouldStoreByteArray()
    {
        var fileBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 };
        
        var questionFile = new QuestionFile
        {
            File = fileBytes
        };

        questionFile.File.Should().NotBeNull();
        questionFile.File.Should().HaveCount(4);
        questionFile.File.Should().BeEquivalentTo(fileBytes);
    }

    [Fact]
    public void QuestionFile_EmptyByteArray_ShouldBeAllowed()
    {
        var questionFile = new QuestionFile
        {
            File = Array.Empty<byte>()
        };

        questionFile.File.Should().NotBeNull();
        questionFile.File.Should().BeEmpty();
    }

    [Fact]
    public void QuestionFile_UploadDate_ShouldBeSet()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        
        var questionFile = new QuestionFile
        {
            UploadDate = today
        };

        questionFile.UploadDate.Should().Be(today);
    }
}
