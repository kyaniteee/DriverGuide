using DriverGuide.Domain.Enums;

namespace DriverGuide.Domain.Models;

public class BulkAnswerItem
{
    public string QuestionId { get; set; } = string.Empty;
    public LicenseCategory QuestionCategory { get; set; }
    public string Question { get; set; } = string.Empty;
    public string CorrectQuestionAnswer { get; set; } = string.Empty;
    public string UserQuestionAnswer { get; set; } = string.Empty;
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public Language QuestionLanguage { get; set; } = Language.PL;
}
