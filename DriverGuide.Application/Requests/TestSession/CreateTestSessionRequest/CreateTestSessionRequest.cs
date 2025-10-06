using DriverGuide.Domain.Enums;

namespace DriverGuide.Application.Requests;

public class CreateTestSessionRequest
{
    public LicenseCategory Category { get; set; }
    public Guid? UserId { get; set; }
}
