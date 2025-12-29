using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using DriverGuide.Infrastructure;
using MediatR;

namespace DriverGuide.Application.Commands;

public class UploadFromPathHandler(IQuestionFileRepository questionFileRepository) : IRequestHandler<UploadFromPathCommand, int>
{
    public async Task<int> Handle(UploadFromPathCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.DirectoryPath))
            throw new ArgumentNullException(nameof(request.DirectoryPath));

        var files = Directory.GetFiles(request.DirectoryPath);
        var uploadedCount = 0;

        foreach (var filePath in files)
        {
            var fileName = Path.GetFileName(filePath);
            var fileBytes = await File.ReadAllBytesAsync(filePath, cancellationToken);

            var questionFile = new QuestionFile
            {
                QuestionFileId = Guid.NewGuid().ToString(),
                Name = fileName,
                File = fileBytes,
                ContentType = DGEnvironment.GetFileMimeType(fileName),
                UploadDate = DateOnly.FromDateTime(DateTime.Now)
            };

            await questionFileRepository.CreateAsync(questionFile);
            uploadedCount++;
        }

        return uploadedCount;
    }
}
