using DriverGuide.Domain.Enums;

namespace DriverGuide.Application.Requests;

public class CreateTestSessionRequest
{
    public DateTimeOffset StartDate { get; set; }
    public LicenseCategory Category { get; set; }
    public Guid? UserId { get; set; }
}
