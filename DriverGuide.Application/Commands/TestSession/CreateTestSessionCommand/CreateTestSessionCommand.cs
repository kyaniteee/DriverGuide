using MediatR;
using DriverGuide.Domain.Enums;

namespace DriverGuide.Application.Commands;

public class CreateTestSessionCommand : IRequest<Guid>
{
    public DateTimeOffset StartDate { get; set; }
    public LicenseCategory Category { get; set; }
    public Guid? UserId { get; set; }
}
