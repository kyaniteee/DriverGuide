using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using DriverGuide.Infrastructure;
using MediatR;

namespace DriverGuide.Application.Commands;

public class CreateQuestionFileHandler(IQuestionFileRepository questionFileRepository) : IRequestHandler<CreateQuestionFileCommand, Guid>
{
    public async Task<Guid> Handle(CreateQuestionFileCommand request, CancellationToken cancellationToken)
    {
        //await using var stream = request.File?.OpenReadStream();
        //using var memoryStream = new MemoryStream();
        //await stream!.CopyToAsync(memoryStream);
        //var fileArray = memoryStream?.ToArray();

        var file = new QuestionFile()
        {
            File = request.File,
            Name = request.FileName,
            QuestionFileId = Guid.NewGuid().ToString(),
            UploadDate = DateOnly.FromDateTime(DateTime.Now),
            ContentType = DGEnvironment.GetFileMimeType(request.FileName!),
        };

        await questionFileRepository.CreateAsync(file);

        return Guid.Parse(file.QuestionFileId!);
    }
}
