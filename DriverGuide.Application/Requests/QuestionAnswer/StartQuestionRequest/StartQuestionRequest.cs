using DriverGuide.Domain.Enums;

namespace DriverGuide.Application.Requests;

public class StartQuestionRequest
{
    public string TestSessionId { get; set; } = string.Empty;
    public string QuestionId { get; set; } = string.Empty;
    public LicenseCategory QuestionCategory { get; set; }
    public string Question { get; set; } = string.Empty;
    public string? CorrectQuestionAnswer { get; set; }
    public DateTimeOffset StartDate { get; set; } = DateTimeOffset.Now;
    public Language QuestionLanguage { get; set; } = Language.PL;
}
