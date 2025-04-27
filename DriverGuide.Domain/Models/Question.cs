namespace DriverGuide.Domain.Models;

/// <summary>
/// Reprezentuje dane pytania.
/// </summary>
public class Question
{
    /// <summary>
    /// Klucz główny rekordu
    /// </summary>
    public int QuestionId { get; set; }

    /// <summary>
    /// Data dodania pytania do bazy danych
    /// </summary>
    public DateOnly DataDodania { get; set; }

    /// <summary>
    /// Numer porządkowy pytania.
    /// </summary>
    public int Lp { get; set; }

    /// <summary>
    /// Numer pytania.
    /// </summary>
    public string? NumerPytania { get; set; }

    /// <summary>
    /// Treść pytania.
    /// </summary>
    public string? Pytanie { get; set; }

    /// <summary>
    /// Odpowiedź A.
    /// </summary>
    public string? OdpowiedzA { get; set; }

    /// <summary>
    /// Odpowiedź B.
    /// </summary>
    public string? OdpowiedzB { get; set; }

    /// <summary>
    /// Odpowiedź C.
    /// </summary>
    public string? OdpowiedzC { get; set; }

    /// <summary>
    /// Poprawna odpowiedź.
    /// </summary>
    public string? PoprawnaOdp { get; set; }

    /// <summary>
    /// Nazwa pliku multimedialnego związanego z pytaniem.
    /// </summary>
    public string? Media { get; set; }

    /// <summary>
    /// Kategoria pytania.
    /// </summary>
    public string? Kategorie { get; set; }

    /// <summary>
    /// Nazwa pliku multimedialnego tłumaczenia migowego (PJM) treści pytania.
    /// </summary>
    public string? NazwaMediaTlumaczenieMigowePJMtrescPyt { get; set; }

    /// <summary>
    /// Nazwa pliku multimedialnego tłumaczenia migowego (PJM) treści odpowiedzi A.
    /// </summary>
    public string? NazwaMediaTlumaczenieMigowePJMtrescA { get; set; }

    /// <summary>
    /// Nazwa pliku multimedialnego tłumaczenia migowego (PJM) treści odpowiedzi B.
    /// </summary>
    public string? NazwaMediaTlumaczenieMigowePJMtrescB { get; set; }

    /// <summary>
    /// Nazwa pliku multimedialnego tłumaczenia migowego (PJM) treści odpowiedzi C.
    /// </summary>
    public string? NazwaMediaTlumaczenieMigowePJMtrescC { get; set; }

    /// <summary>
    /// Treść pytania w języku angielskim.
    /// </summary>
    public string? PytanieENG { get; set; }

    /// <summary>
    /// Odpowiedź A w języku angielskim.
    /// </summary>
    public string? OdpowiedzAENG { get; set; }

    /// <summary>
    /// Odpowiedź B w języku angielskim.
    /// </summary>
    public string? OdpowiedzBENG { get; set; }

    /// <summary>
    /// Odpowiedź C w języku angielskim.
    /// </summary>
    public string? OdpowiedzCENG { get; set; }

    /// <summary>
    /// Treść pytania w języku niemieckim.
    /// </summary>
    public string? PytanieDE { get; set; }

    /// <summary>
    /// Odpowiedź A w języku niemieckim.
    /// </summary>
    public string? OdpowiedzADE { get; set; }

    /// <summary>
    /// Odpowiedź B w języku niemieckim.
    /// </summary>
    public string? OdpowiedzBDE { get; set; }

    /// <summary>
    /// Odpowiedź C w języku niemieckim.
    /// </summary>
    public string? OdpowiedzCDE { get; set; }

    /// <summary>
    /// Treść pytania w języku ukraińskim.
    /// </summary>
    public string? PytanieUA { get; set; }

    /// <summary>
    /// Odpowiedź A w języku ukraińskim.
    /// </summary>
    public string? OdpowiedzAUA { get; set; }

    /// <summary>
    /// Odpowiedź B w języku ukraińskim.
    /// </summary>
    public string? OdpowiedzBUA { get; set; }

    /// <summary>
    /// Odpowiedź C w języku ukraińskim.
    /// </summary>
    public string? OdpowiedzCUA { get; set; }
}