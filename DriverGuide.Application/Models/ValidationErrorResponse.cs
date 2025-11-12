namespace DriverGuide.Application.Models;

/// <summary>
/// Represents a validation error response returned to the client.
/// Contains detailed information about validation failures.
/// </summary>
public class ValidationErrorResponse
{
    /// <summary>
    /// Error message summary
    /// </summary>
    public string Message { get; set; } = "Validation failed";

    /// <summary>
    /// Dictionary of field-specific errors where key is the field name
    /// and value is the list of error messages for that field
    /// </summary>
    public Dictionary<string, List<string>> Errors { get; set; } = new();

    /// <summary>
    /// Timestamp when the error occurred
    /// </summary>
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
}
