using DriverGuide.Domain.Enums;

namespace DriverGuide.Domain.DTOs
{
    public class TestSummaryDto
    {
        public string TestSessionId { get; set; } = string.Empty;
        public LicenseCategory Category { get; set; }
        public double? Result { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public List<QuestionSummaryDto> Questions { get; set; } = new();
    }
}
