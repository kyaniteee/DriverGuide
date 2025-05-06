using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DriverGuide.UI.Domain;

/// <summary>
/// Reprezentuje kategorie prawa jazdy.
/// </summary>
[Flags]
public enum DrivingLicenseCategory
{
    /// <summary>
    /// Uprawnienia dla motorowerów, czterokołowców lekkich i zespołów pojazdów. Wymagany wiek: 14 lat.
    /// </summary>
    [Display(Name = "AM - Motorower, czterokołowiec lekki")]
    [Description("AM - Motorower, czterokołowiec lekki")]
    AM = 1 << 0, // 1

    /// <summary>
    /// Uprawnienia dla motocykli o poj. skokowej do 125 cm3, mocy do 11 kW, motocykli trójkołowych o mocy do 15 kW i zespołów pojazdów. Wymagany wiek: 16 lat.
    /// </summary>
    [Display(Name = "A1 - Motocykl do 125 cm3")]
    [Description("A1 - Motocykl do 125 cm3")]
    A1 = 1 << 1, // 2

    /// <summary>
    /// Uprawnienia dla motocykli o mocy do 35 kW i stosunku mocy do masy własnej do 0,2 kW/kg, motocykli trójkołowych do 15 kW i zespołów pojazdów. Wymagany wiek: 18 lat.
    /// </summary>
    [Display(Name = "A2 - Motocykl do 35 kW")]
    [Description("A2 - Motocykl do 35 kW")]
    A2 = 1 << 2, // 4

    /// <summary>
    /// Uprawnienia dla każdego motocykla i zespołów pojazdów. Wymagany wiek: 20/24 lata.
    /// </summary>
    [Display(Name = "A - Każdy motocykl")]
    [Description("A - Każdy motocykl")]
    A = 1 << 3, // 8

    /// <summary>
    /// Uprawnienia dla czterokołowców i pojazdów kategorii AM. Wymagany wiek: 16 lat.
    /// </summary>
    [Display(Name = "B1 - Czterokołowiec")]
    [Description("B1 - Czterokołowiec")]
    B1 = 1 << 4, // 16

    /// <summary>
    /// Uprawnienia dla pojazdów samochodowych do 3,5 t, zespołów pojazdów, ciągników rolniczych i motocykli (spełniających określone warunki). Wymagany wiek: 18 lat.
    /// </summary>
    [Display(Name = "B - Pojazd samochodowy do 3,5 t")]
    [Description("B - Pojazd samochodowy do 3,5 t")]
    B = 1 << 5, // 32

    /// <summary>
    /// Uprawnienia dla pojazdów kategorii B z przyczepą. Wymagany wiek: 18 lat.
    /// </summary>
    [Display(Name = "B+E - Pojazd kat. B z przyczepą")]
    [Description("B+E - Pojazd kat. B z przyczepą")]
    B_E = 1 << 6, // 64

    /// <summary>
    /// Uprawnienia dla pojazdów samochodowych od 3,5 t do 7,5 t. Wymagany wiek: 18 lat.
    /// </summary>
    [Display(Name = "C1 - Pojazd od 3,5 t do 7,5 t")]
    [Description("C1 - Pojazd od 3,5 t do 7,5 t")]
    C1 = 1 << 7, // 128

    /// <summary>
    /// Uprawnienia dla pojazdów samochodowych o dmc powyżej 3,5 t. Wymagany wiek: 21 lat.
    /// </summary>
    [Display(Name = "C - Pojazd powyżej 3,5 t")]
    [Description("C - Pojazd powyżej 3,5 t")]
    C = 1 << 8, // 256

    /// <summary>
    /// Uprawnienia dla zespołów pojazdów kategorii C1 z przyczepą. Wymagany wiek: 18 lat.
    /// </summary>
    [Display(Name = "C1+E - Pojazd kat. C1 z przyczepą")]
    [Description("C1+E - Pojazd kat. C1 z przyczepą")]
    C1_E = 1 << 9, // 512

    /// <summary>
    /// Uprawnienia dla pojazdów kategorii C z przyczepą. Wymagany wiek: 21 lat.
    /// </summary>
    [Display(Name = "C+E - Pojazd kat. C z przyczepą")]
    [Description("C+E - Pojazd kat. C z przyczepą")]
    C_E = 1 << 10, // 1024

    /// <summary>
    /// Uprawnienia dla autobusów do 17 osób. Wymagany wiek: 21 lat.
    /// </summary>
    [Display(Name = "D1 - Autobus do 17 osób")]
    [Description("D1 - Autobus do 17 osób")]
    D1 = 1 << 11, // 2048

    /// <summary>
    /// Uprawnienia dla autobusów. Wymagany wiek: 24 lata.
    /// </summary>
    [Display(Name = "D - Autobus")]
    [Description("D - Autobus")]
    D = 1 << 12, // 4096

    /// <summary>
    /// Uprawnienia dla zespołów pojazdów kategorii D1 z przyczepą. Wymagany wiek: 21 lat.
    /// </summary>
    [Display(Name = "D1+E - Pojazd kat. D1 z przyczepą")]
    [Description("D1+E - Pojazd kat. D1 z przyczepą")]
    D1_E = 1 << 13, // 8192

    /// <summary>
    /// Uprawnienia dla pojazdów kategorii D z przyczepą. Wymagany wiek: 24 lata.
    /// </summary>
    [Display(Name = "D+E - Pojazd kat. D z przyczepą")]
    [Description("D+E - Pojazd kat. D z przyczepą")]
    D_E = 1 << 14, // 16384

    /// <summary>
    /// Uprawnienia dla ciągników rolniczych i pojazdów wolnobieżnych. Wymagany wiek: 16 lat.
    /// </summary>
    [Display(Name = "T - Ciągnik rolniczy")]
    [Description("T - Ciągnik rolniczy")]
    T = 1 << 15, // 32768

    /// <summary>
    /// Uprawnienia dla tramwajów. Wymagany wiek: 21 lat.
    /// </summary>
    [Display(Name = "Tramwaj")]
    [Description("Tramwaj")]
    Tramwaj = 1 << 16 // 65536
}
