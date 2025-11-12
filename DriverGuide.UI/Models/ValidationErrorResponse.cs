namespace DriverGuide.UI.Models;

/// <summary>
/// Represents a validation error response from the API
/// </summary>
public class ValidationErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public Dictionary<string, List<string>> Errors { get; set; } = new();
    public DateTimeOffset Timestamp { get; set; }
}
