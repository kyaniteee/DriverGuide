using DriverGuide.Domain.Enums;

namespace DriverGuide.Domain.Models;

public class QuestionAnswer
{
    /// <summary>
    /// Klucz główny
    /// </summary>
    public string? QuestionAnswerId { get; set; }
    /// <summary>
    /// Relacja do testu
    /// </summary>
    public string? TestSessionId { get; set; }
    /// <summary>
    /// Relacja do zapytania
    /// </summary>
    public string? QuestionId { get; set; }
    /// <summary>
    /// Określa kategorie pytania
    /// </summary>
    public DrivingLicenseCategory QuestionCategory { get; set; }
    /// <summary>
    /// Zapytanie
    /// </summary>
    public string? Question { get; set; }
    /// <summary>
    /// Poprawna odpowiedź
    /// </summary>
    public string? CorrectQuestionAnswer { get; set; }
    /// <summary>
    /// Odpowiedź użytkownika
    /// </summary>
    public string? UserQuestionAnswer { get; set; }
    /// <summary>
    /// Data rozpoczęcia pytania
    /// </summary>
    public DateTimeOffset StartDate { get; set; }
    /// <summary>
    /// Data zakończenia pytania
    /// </summary>
    public DateTimeOffset? EndDate { get; set; }
    /// <summary>
    /// Języj odpowiedzi
    /// </summary>
    public Language QuestionLanguage { get; set; }
}