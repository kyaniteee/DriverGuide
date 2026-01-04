using DriverGuide.Domain.Enums;
using DriverGuide.Domain.Models;

namespace DriverGuide.Domain.Models;

/// <summary>
/// Model reprezentujący pojedynczą odpowiedź użytkownika na pytanie egzaminacyjne w ramach sesji testowej.
/// Przechowuje informacje o pytaniu, poprawnej odpowiedzi oraz odpowiedzi udzielonej przez użytkownika.
/// </summary>
public class QuestionAnswer
{
    /// <summary>
    /// Unikalny identyfikator odpowiedzi na pytanie.
    /// Generowany automatycznie jako GUID w formacie string.
    /// </summary>
    public string? QuestionAnswerId { get; set; }

    /// <summary>
    /// Identyfikator sesji testowej, do której należy ta odpowiedź.
    /// Powiązanie z modelem TestSession.
    /// </summary>
    public string? TestSessionId { get; set; }

    /// <summary>
    /// Identyfikator pytania egzaminacyjnego.
    /// Odniesienie do tabeli Questions (QuestionId).
    /// </summary>
    public int QuestionId { get; set; }

    /// <summary>
    /// Kategoria prawa jazdy, której dotyczy pytanie.
    /// Używana do filtrowania i kategoryzacji pytań w raportach.
    /// </summary>
    public LicenseCategory QuestionCategory { get; set; }

    /// <summary>
    /// Treść pytania w wybranym języku.
    /// Przechowywana dla łatwego dostępu bez konieczności ponownego pobierania z tabeli Questions.
    /// </summary>
    public string? QuestionText { get; set; }

    /// <summary>
    /// Poprawna odpowiedź na pytanie (A, B, C lub Tak/Nie).
    /// Używana do sprawdzenia poprawności odpowiedzi użytkownika.
    /// </summary>
    public string? CorrectQuestionAnswer { get; set; }

    /// <summary>
    /// Odpowiedź udzielona przez użytkownika (A, B, C lub Tak/Nie).
    /// Null jeśli użytkownik nie udzielił odpowiedzi (timeout).
    /// </summary>
    public string? UserQuestionAnswer { get; set; }

    /// <summary>
    /// Data i czas rozpoczęcia wyświetlania pytania użytkownikowi.
    /// Używana do obliczenia czasu potrzebnego na udzielenie odpowiedzi.
    /// </summary>
    public DateTimeOffset StartDate { get; set; }

    /// <summary>
    /// Data i czas udzielenia odpowiedzi lub wygaśnięcia czasu na odpowiedź.
    /// Null jeśli pytanie jest aktualnie wyświetlane.
    /// </summary>
    public DateTimeOffset? EndDate { get; set; }

    /// <summary>
    /// Język, w jakim pytanie zostało wyświetlone użytkownikowi.
    /// Może być PL, ENG, DE lub UA.
    /// </summary>
    public Language QuestionLanguage { get; set; }
}