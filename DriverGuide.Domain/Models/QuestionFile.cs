namespace DriverGuide.Domain.Models;

/// <summary>
/// Reprezentuje załącznik powiązany z pytaniem.
/// </summary>
public class QuestionFile
{
    /// <summary>
    /// Unikalny identyfikator załącznika.
    /// </summary>
    public string? QuestionFileId { get; set; }

    /// <summary>
    /// Pozwala przeglądarce lub aplikacji wiedzieć, jakiego rodzaju dane zawiera plik.
    /// </summary>
    public string? ContentType { get; set; }

    /// <summary>
    /// Data dodania załącznika.
    /// </summary>
    public DateOnly UploadDate { get; set; }

    /// <summary>
    /// Nazwa pliku załącznika.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Zawartość pliku załącznika.
    /// </summary>
    public byte[]? File { get; set; }
}
