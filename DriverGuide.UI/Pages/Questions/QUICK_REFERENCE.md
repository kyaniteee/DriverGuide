# ?? Questions Page - Quick Reference

## ?? Podstawowe Informacje

**Lokalizacja**: `/pytania`  
**Komponenty**:
- `Questions.razor` - UI
- `Questions.razor.cs` - Logika
- `Questions.razor.css` - Style

---

## ?? Kluczowe Funkcje

### ?? Filtrowanie
```csharp
// Search
searchTerm = "jakieœ pytanie";

// Category
selectedCategory = "B";

// Sort
sortMode = SortMode.Newest;
```

### ??? Tryby Widoku
```csharp
public enum ViewMode
{
    Cards,      // Default - Kafelki
    List,       // Lista
    Compact     // Tabela
}
```

### ?? Paginacja
```csharp
currentPage = 1;
questionsPerPage = 20;  // 10/20/50/100
```

---

## ?? Design Tokens

```css
/* Colors */
--primary: #667eea;
--success: #48bb78;
--danger: #e53e3e;

/* Spacing */
--radius: 12px;
--radius-lg: 16px;

/* Shadows */
--shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
--shadow-lg: 0 10px 25px rgba(0, 0, 0, 0.15);
```

---

## ??? Struktura Danych

### Question Model
```csharp
public class Question
{
    public int QuestionId { get; set; }
    public string? Pytanie { get; set; }
    public string? OdpowiedzA { get; set; }
    public string? OdpowiedzB { get; set; }
    public string? OdpowiedzC { get; set; }
    public string? PoprawnaOdp { get; set; }
    public string? Media { get; set; }
    public string? Kategorie { get; set; }
    public DateOnly DataDodania { get; set; }
}
```

---

## ?? Computed Properties

### Pipeline
```
allQuestions
    ?
FilteredQuestions (search + category)
    ?
FilteredAndSortedQuestions (sort mode)
    ?
PaginatedQuestions (current page + per page)
```

---

## ?? States

### Loading
```razor
@if (isLoading)
{
    <div class="loading-state">
        <div class="loading-spinner"></div>
        <p>£adowanie pytañ...</p>
    </div>
}
```

### Error
```razor
@if (loadFailed)
{
    <div class="error-state">
        <h3>B³¹d ³adowania</h3>
        <button @onclick="ReloadPage">Odœwie¿</button>
    </div>
}
```

### Empty
```razor
@if (!FilteredAndSortedQuestions.Any())
{
    <div class="empty-state">
        <h3>Brak wyników</h3>
        <button @onclick="ClearFilters">Wyczyœæ filtry</button>
    </div>
}
```

---

## ?? CSS Classes Reference

### Card View
```css
.questions-grid          /* Grid container */
.question-card           /* Card container */
.card-header             /* Top section */
.question-number         /* #123 */
.media-badge             /* Video icon */
.card-body               /* Main content */
.question-text           /* Question */
.question-categories     /* Category tags */
.card-details            /* Expanded answers */
.answer-item             /* Single answer */
.answer-item.correct     /* Correct answer */
.card-footer             /* Expand button */
```

### List View
```css
.questions-list          /* List container */
.question-list-item      /* Item container */
.list-item-header        /* Clickable header */
.item-number             /* #123 badge */
.media-indicator         /* Play button */
.item-question           /* Question text */
.item-meta               /* Categories + preview */
.list-item-details       /* Expanded section */
.detail-answer           /* Answer row */
.detail-answer.is-correct /* Correct answer */
```

### Table View
```css
.questions-table-wrapper /* Wrapper */
.questions-table         /* Table */
.col-id                  /* # column */
.col-question            /* Question column */
.col-answer              /* Answer column */
.col-categories          /* Categories column */
.col-media               /* Media column */
.table-category-tag      /* Category badge */
.media-btn               /* Play button */
```

---

## ?? Key Methods

### Filtering
```csharp
void SelectCategory(string? category)
void ClearSearch()
void ClearFilters()
```

### View Management
```csharp
void SetViewMode(ViewMode mode)
void ToggleQuestionDetails(int questionId)
```

### Pagination
```csharp
void ChangePage(int page)
int GetPageStartIndex()
int GetPageEndIndex()
List<int> GetVisiblePages()
```

### Media
```csharp
Task OpenMedia(Question q)
Task CloseMedia()
Task CloseMediaFromEsc()  // JS Invokable
```

### Utilities
```csharp
List<string> GetQuestionCategoriesList(Question q)
string GetCorrectAnswerText(Question q)
string GetCategoryIcon(string category)
int GetCategoryCount(string category)
```

---

## ?? Keyboard Shortcuts

- `Tab` - Navigate through elements
- `Enter` - Expand/collapse question
- `Esc` - Close modal
- `Arrow Keys` - Navigate pagination

---

## ?? Breakpoints

```css
Desktop:  > 1200px
Tablet:   768px - 1200px
Mobile:   < 768px
Small:    < 480px
```

---

## ?? Animations

```css
fadeIn      - Page load
spin        - Loading spinner
slideDown   - Card expand
scaleIn     - Modal open
```

---

## ?? Related Pages

- `/` - Home (link to questions)
- `/test` - Test selection
- `/quiz/{category}` - Quiz

---

## ?? Metrics

**Target Performance:**
- Load time: < 2s
- Search: < 100ms
- View switch: < 50ms
- Modal: < 200ms

**Target Engagement:**
- Time on page: 3-5 min
- Questions viewed: 15-25
- Search usage: 60%
- Filter usage: 40%

---

## ?? Common Issues

### Issue: Pagination nie resetuje siê
**Solution**: `currentPage = 1` w `SelectCategory` i `ClearSearch`

### Issue: Modal nie zamyka siê ESC
**Solution**: SprawdŸ `modalKey.registerEsc` w JS

### Issue: Search lag
**Solution**: U¿yj debounce (opcjonalne)

### Issue: Media nie ³aduje
**Solution**: SprawdŸ `/QuestionFile/GetQuestionFileByName` endpoint

---

## ? Quick Checklist

Przed deploymentem:
- [ ] Build successful
- [ ] Wszystkie 3 widoki dzia³aj¹
- [ ] Filtrowanie dzia³a
- [ ] Paginacja dzia³a
- [ ] Modal dzia³a
- [ ] Responsive na mobile
- [ ] Accessibility (keyboard)
- [ ] Loading states OK
- [ ] Error handling OK

---

**Ostatnia aktualizacja**: 2024  
**Wersja**: 2.0.0
