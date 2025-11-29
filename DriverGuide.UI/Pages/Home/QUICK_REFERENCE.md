# Home Page - Quick Reference

## ?? Szybki Start

### Struktura Plików
```
DriverGuide.UI/Pages/Home/
??? Home.razor          # G³ówny komponent
??? Home.razor.css      # Style (scoped)
??? README.md           # Pe³na dokumentacja
```

## ?? Edycja Treœci

### Zmiana Statystyk
```csharp
// Home.razor - @code section
private const int TotalQuestions = 5000;  // Zmieñ liczbê pytañ
```

```html
<!-- Home.razor - Stats Section -->
<div class="stat-number">50K+</div>     <!-- Liczba u¿ytkowników -->
<div class="stat-number">94%</div>      <!-- Zdawalnoœæ -->
<div class="stat-number">12,453</div>   <!-- Aktywni u¿ytkownicy (floating card) -->
```

### Zmiana Popularnych Kategorii
```csharp
// Home.razor - @code section
private LicenseCategory[] PopularCategories = new[]
{
    LicenseCategory.B,   // Dodaj/usuñ kategorie
    LicenseCategory.A,
    LicenseCategory.A2,
    // ...
};
```

### Zmiana Linków Rz¹dowych
```html
<!-- Home.razor - Links Section -->
<a href="https://www.example.gov.pl" target="_blank" class="link-card">
    <div class="link-icon">
        <i class="bi bi-building"></i>  <!-- Zmieñ ikonê -->
    </div>
    <div class="link-content">
        <h4>Tytu³</h4>                  <!-- Zmieñ tytu³ -->
        <p>Opis</p>                     <!-- Zmieñ opis -->
    </div>
</a>
```

## ?? Customizacja Wygl¹du

### Kolory
```css
/* Home.razor.css - :root */
--primary: #2d6cdf;           /* G³ówny kolor */
--primary-dark: #1b417a;      /* Ciemniejszy odcieñ */
--primary-light: #5a8eef;     /* Jaœniejszy odcieñ */
```

### Spacing
```css
/* Home.razor.css */
.hero-section {
    padding: 4rem 6rem;       /* Zmieñ padding hero */
}

.features-section {
    padding: 6rem 6rem;       /* Zmieñ padding sekcji */
}
```

### Animacje
```css
/* Home.razor.css */
--transition: all 0.3s ease;  /* Szybkoœæ animacji */

/* Wy³¹cz animacjê floating cards */
.card-1, .card-2, .card-3 {
    animation: none;
}
```

## ?? Breakpoints

| Breakpoint | Szerokoœæ | Zmiany |
|------------|-----------|--------|
| Desktop XL | > 1400px  | Domyœlny layout |
| Desktop    | 1200px    | Mniejszy padding, 2 kolumny w gridach |
| Tablet     | 992px     | Hero 1 kolumna, ukryty hero image |
| Mobile     | 768px     | Wszystko 1 kolumna, full-width buttons |
| Small      | 576px     | Minimalne paddingi |

## ?? Nawigacja

### Metody nawigacji
```csharp
NavigateToTest()         // ? /test
NavigateToProfile()      // ? /profile  
NavigateToRegister()     // ? /register
StartGuestTest()         // ? /quiz/B
StartCategoryTest(cat)   // ? /quiz/{category}
```

### Dodanie nowej nawigacji
```csharp
// 1. Dodaj metodê w @code
private void NavigateToNewPage()
{
    Navigation.NavigateTo("/new-page");
}

// 2. U¿yj w przycisku
<button class="btn btn-hero-primary" @onclick="NavigateToNewPage">
    Click me
</button>
```

## ??? Obrazy Kategorii

### Zmiana Ÿród³a obrazów
```csharp
// Home.razor - GetCategoryImage()
return category switch
{
    LicenseCategory.B => "YOUR_IMAGE_URL",
    // Lub œcie¿ka lokalna:
    LicenseCategory.B => "/images/categories/b.jpg",
    // ...
};
```

### Parametry Unsplash
```
?w=600          # Szerokoœæ
&h=450          # Wysokoœæ
&fit=crop       # Sposób dopasowania
&q=85           # Jakoœæ (1-100)
```

## ?? Ikony Bootstrap

### Najczêœciej u¿ywane
```html
<i class="bi bi-rocket-takeoff-fill"></i>  <!-- Start/Launch -->
<i class="bi bi-play-circle-fill"></i>     <!-- Play/Start -->
<i class="bi bi-graph-up"></i>             <!-- Stats/Analytics -->
<i class="bi bi-check-circle-fill"></i>    <!-- Success/Check -->
<i class="bi bi-shield-check"></i>         <!-- Security -->
<i class="bi bi-trophy-fill"></i>          <!-- Achievement -->
<i class="bi bi-people-fill"></i>          <!-- Users/Community -->
<i class="bi bi-clock-fill"></i>           <!-- Time/Recent -->
```

### Pe³na lista
[Bootstrap Icons](https://icons.getbootstrap.com/)

## ?? Klasy CSS Utility

### Buttons
```css
.btn-hero-primary      /* G³ówny przycisk w hero */
.btn-hero-secondary    /* Drugorzêdny przycisk w hero */
.btn-category          /* Przycisk w karcie kategorii */
.btn-cta              /* Wielki CTA button */
.btn-outline-primary  /* Outline button */
```

### Sections
```css
.hero-section         /* Hero z 2 kolumnami */
.stats-section        /* Sekcja statystyk z gradientem */
.features-section     /* Grid 3x2 z funkcjami */
.categories-section   /* Popularne kategorie */
.how-it-works-section /* Kroki 1-2-3 */
.links-section        /* Linki rz¹dowe */
.cta-section         /* Koñcowy CTA */
```

### Cards
```css
.floating-card        /* Floating card w hero */
.feature-card         /* Karta funkcjonalnoœci */
.category-card-home   /* Karta kategorii */
.link-card           /* Karta z linkiem */
.step-item           /* Element kroku (how it works) */
```

## ?? Common Issues

### Obrazki siê nie ³aduj¹
```csharp
// SprawdŸ URL w GetCategoryImage()
// Upewnij siê ¿e Unsplash URLs s¹ poprawne
// Lub u¿yj lokalnych obrazów:
return "/images/categories/b.jpg";
```

### Nawigacja nie dzia³a
```csharp
// Upewnij siê ¿e @inject jest na górze pliku:
@inject NavigationManager Navigation

// SprawdŸ czy route istnieje w docelowej stronie:
@page "/test"  // w TestSelection.razor
```

### Style nie dzia³aj¹
```html
<!-- Upewnij siê ¿e plik CSS ma .razor.css -->
Home.razor.css  ?
Home.css        ?

<!-- SprawdŸ czy projekt zosta³ zbudowany -->
dotnet build
```

### AuthorizeView nie dzia³a
```csharp
// Dodaj using na górze:
@inject AuthenticationStateProvider AuthStateProvider

// Upewnij siê ¿e AuthenticationStateProvider jest zarejestrowany
// w Program.cs
```

## ?? Checklist przed Commitem

- [ ] Build bez b³êdów (`dotnet build`)
- [ ] Wszystkie linki dzia³aj¹
- [ ] Obrazki siê ³aduj¹
- [ ] Responsive sprawdzony (DevTools)
- [ ] Animations dzia³aj¹ p³ynnie
- [ ] Nie ma console errors
- [ ] README zaktualizowane (jeœli potrzeba)

## ?? Deployment

```bash
# 1. Build projektu
dotnet build

# 2. Publish
dotnet publish -c Release

# 3. SprawdŸ build artifacts
# w DriverGuide.UI/bin/Release/net8.0/publish/
```

## ?? Przydatne Linki

- [Blazor Docs](https://docs.microsoft.com/en-us/aspnet/core/blazor/)
- [Bootstrap Icons](https://icons.getbootstrap.com/)
- [Unsplash Source](https://source.unsplash.com/)
- [Can I Use](https://caniuse.com/) - CSS support
- [CSS Tricks](https://css-tricks.com/) - CSS guides
- [MDN Web Docs](https://developer.mozilla.org/) - HTML/CSS/JS reference

## ?? Pro Tips

1. **Glassmorphism**: `backdrop-filter: blur(10px)` + semi-transparent bg
2. **Smooth animations**: U¿ywaj `transform` i `opacity` (GPU accelerated)
3. **Loading states**: Dodaj `loading="lazy"` do obrazków
4. **Accessibility**: Zawsze dodawaj `alt` text do obrazków
5. **Performance**: Optymalizuj rozmiar obrazków (Unsplash query params)

---

**Need help?** Check the full README.md for detailed documentation.
