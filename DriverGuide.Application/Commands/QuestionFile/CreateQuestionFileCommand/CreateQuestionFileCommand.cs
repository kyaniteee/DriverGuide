using MediatR;

namespace DriverGuide.Application.Commands;

public class CreateQuestionFileCommand : IRequest<Guid>
{
    public byte[]? File { get; set; }
    public string? FileName { get; set; }
}
