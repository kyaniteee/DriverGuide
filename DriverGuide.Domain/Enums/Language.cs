using System.ComponentModel;

namespace DriverGuide.Domain.Enums;

/// <summary>
/// Enumeracja reprezentująca języki dostępne w systemie do wyświetlania pytań egzaminacyjnych.
/// Zgodna z wymogami WORD dotyczącymi wielojęzyczności egzaminów teoretycznych.
/// </summary>
public enum Language
{
    /// <summary>
    /// Język polski - domyślny język systemu.
    /// Wszystkie pytania są dostępne w języku polskim.
    /// </summary>
    [Description("Polski")]
    PL,

    /// <summary>
    /// Język angielski - dla obcokrajowców znających angielski.
    /// Dostępność pytań w tym języku zależy od tłumaczeń w bazie danych.
    /// </summary>
    [Description("Angielski")]
    ENG,

    /// <summary>
    /// Język niemiecki - dla obcokrajowców znających niemiecki.
    /// Dostępność pytań w tym języku zależy od tłumaczeń w bazie danych.
    /// </summary>
    [Description("Niemiecki")]
    DE,

    /// <summary>
    /// Język ukraiński - dla obcokrajowców znających ukraiński.
    /// Dostępność pytań w tym języku zależy od tłumaczeń w bazie danych.
    /// </summary>
    [Description("Ukraiński")]
    UA
}
