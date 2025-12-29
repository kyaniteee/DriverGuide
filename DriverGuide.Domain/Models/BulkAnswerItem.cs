using DriverGuide.Domain.Enums;

namespace DriverGuide.Domain.Models;

/// <summary>
/// Model reprezentujący pojedynczą pozycję w zbiorczym wysyłaniu odpowiedzi.
/// Używany przy masowym zapisie odpowiedzi na zakończenie testu (tryb offline/cache).
/// </summary>
public class BulkAnswerItem
{
    /// <summary>
    /// Identyfikator pytania egzaminacyjnego.
    /// Odniesienie do tabeli Questions.
    /// </summary>
    public string QuestionId { get; set; } = string.Empty;

    /// <summary>
    /// Kategoria prawa jazdy, której dotyczy pytanie.
    /// </summary>
    public LicenseCategory QuestionCategory { get; set; }

    /// <summary>
    /// Treść pytania w wybranym języku.
    /// </summary>
    public string Question { get; set; } = string.Empty;

    /// <summary>
    /// Poprawna odpowiedź na pytanie (A, B, C lub Tak/Nie).
    /// </summary>
    public string CorrectQuestionAnswer { get; set; } = string.Empty;

    /// <summary>
    /// Odpowiedź udzielona przez użytkownika (A, B, C lub Tak/Nie).
    /// </summary>
    public string UserQuestionAnswer { get; set; } = string.Empty;

    /// <summary>
    /// Data i czas rozpoczęcia wyświetlania pytania.
    /// </summary>
    public DateTimeOffset StartDate { get; set; }

    /// <summary>
    /// Data i czas udzielenia odpowiedzi.
    /// </summary>
    public DateTimeOffset EndDate { get; set; }

    /// <summary>
    /// Język wyświetlenia pytania.
    /// Domyślnie język polski (PL).
    /// </summary>
    public Language QuestionLanguage { get; set; } = Language.PL;
}
