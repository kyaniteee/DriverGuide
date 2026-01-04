namespace DriverGuide.Domain.Models;

/// <summary>
/// Model reprezentujący sesję testową użytkownika.
/// Przechowuje informacje o przeprowadzonym teście egzaminacyjnym, czasie trwania i wyniku końcowym.
/// </summary>
public class TestSession
{
    /// <summary>
    /// Unikalny identyfikator sesji testowej.
    /// Generowany automatycznie jako GUID w formacie string.
    /// </summary>
    public string? TestSessionId { get; set; }

    /// <summary>
    /// Identyfikator użytkownika przeprowadzającego test.
    /// Null dla użytkowników niezalogowanych (tryb gościa).
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Data i czas rozpoczęcia sesji testowej.
    /// Automatycznie ustawiana podczas tworzenia sesji.
    /// </summary>
    public DateTimeOffset StartDate { get; set; }

    /// <summary>
    /// Data i czas zakończenia sesji testowej.
    /// Null jeśli test jest w trakcie wykonywania.
    /// Ustawiana automatycznie po przesłaniu ostatniej odpowiedzi.
    /// </summary>
    public DateTimeOffset? EndDate { get; set; }

    /// <summary>
    /// Wynik końcowy testu wyrażony w procentach (0-100).
    /// Null jeśli test nie został jeszcze zakończony.
    /// Obliczany jako (liczba poprawnych odpowiedzi / liczba wszystkich pytań) * 100.
    /// </summary>
    public double? Result { get; set; }

    /// <summary>
    /// Kolekcja odpowiedzi na pytania w ramach sesji testowej.
    /// </summary>
    public ICollection<QuestionAnswer>? QuestionAnswers { get; set; }
}