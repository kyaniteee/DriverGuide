namespace DriverGuide.Domain.DTOs;

public class TestSessionResultDto
{
    public string TestSessionId { get; set; } = string.Empty;
    public double? Result { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public bool IsCompleted => EndDate.HasValue && Result.HasValue;
    public bool IsPassed => IsCompleted && Result >= 68;
    public bool IsFailed => IsCompleted && Result < 68;
    public string Duration => CalculateDuration();
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }

    private string CalculateDuration()
    {
        if (EndDate == null) return "W trakcie...";
        var duration = EndDate.Value - StartDate;
        return $"{(int)duration.TotalMinutes}:{duration.Seconds:D2}";
    }
}
