namespace DriverGuide.Domain.Models;

/// <summary>
/// Model reprezentujący plik multimedialny powiązany z pytaniem egzaminacyjnym.
/// Przechowuje obrazy, filmy wideo lub inne pliki używane w pytaniach teoretycznych.
/// </summary>
public class QuestionFile
{
    /// <summary>
    /// Unikalny identyfikator pliku w systemie.
    /// Generowany automatycznie jako GUID w formacie string.
    /// </summary>
    public string? QuestionFileId { get; set; }

    /// <summary>
    /// Nazwa pliku z rozszerzeniem (np. "pytanie_001.jpg", "video_002.mp4").
    /// Używana do identyfikacji pliku i określenia jego typu.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Zawartość pliku w postaci tablicy bajtów.
    /// Przechowywana bezpośrednio w bazie danych jako VARBINARY(MAX).
    /// </summary>
    public byte[]? File { get; set; }

    /// <summary>
    /// Typ MIME pliku (np. "image/jpeg", "video/mp4", "image/png").
    /// Określa format pliku i sposób jego wyświetlenia w przeglądarce.
    /// </summary>
    public string? ContentType { get; set; }

    /// <summary>
    /// Data dodania pliku do systemu.
    /// Domyślnie ustawiana na aktualną datę podczas uploadu.
    /// </summary>
    public DateOnly UploadDate { get; set; }
}
