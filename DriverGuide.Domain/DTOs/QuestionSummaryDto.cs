namespace DriverGuide.Domain.DTOs
{
    public class QuestionSummaryDto
    {
        public string QuestionId { get; set; } = string.Empty;
        public string QuestionText { get; set; } = string.Empty;
        public string? MediaFileName { get; set; }
        public string CorrectAnswer { get; set; } = string.Empty;
        public string? UserAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public List<string> AvailableAnswers { get; set; } = new();
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public int Points { get; set; }
    }
}
