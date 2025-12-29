using MediatR;

namespace DriverGuide.Application.Commands;

public class UploadFromPathCommand : IRequest<int>
{
    public string? DirectoryPath { get; set; }
}
