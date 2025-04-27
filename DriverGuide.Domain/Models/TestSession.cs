namespace DriverGuide.Domain.Models;

public class TestSession
{
    /// <summary>
    /// Klucz główny
    /// </summary>
    public string? TestSessionId { get; set; }
    /// <summary>
    /// Wynik testu
    /// </summary>
    public double? Result { get; set; }
    /// <summary>
    /// Data rozpoczęcia testu
    /// </summary>
    public DateTimeOffset StartDate { get; set; }
    /// <summary>
    /// Data zakończenia testu
    /// </summary>
    public DateTimeOffset? EndDate { get; set; }
    /// <summary>
    /// Identyfikator użytkownika rozpoczynającego test
    /// </summary>
    public Guid? UserId { get; set; }
}