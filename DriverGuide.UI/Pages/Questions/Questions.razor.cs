using DriverGuide.Domain.Enums;
using DriverGuide.Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace DriverGuide.UI.Pages.Questions
{
    public partial class Questions : ComponentBase, IAsyncDisposable
    {
        [Inject] private HttpClient HttpClient { get; set; } = default!;
        [Inject] private IJSRuntime JS { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private DotNetObjectReference<Questions>? _selfRef;

        // Data
        private List<Question> allQuestions = new();
        private HashSet<string> AvailableCategories = new();

        // Filter state
        private string searchTerm = string.Empty;
        private string? selectedCategory = null;
        private SortMode sortMode = SortMode.Newest;
        private ViewMode viewMode = ViewMode.Cards;

        // Pagination
        private int currentPage = 1;
        private int questionsPerPage = 20;

        // UI state
        private bool isLoading = true;
        private bool loadFailed = false;
        private int? expandedQuestionId = null;

        // Modal state
        private bool isMediaOpen = false;
        private string? mediaUrl;
        private string? mediaContentType;
        private Question? currentMediaQuestion;

        // Computed properties
        private IEnumerable<Question> FilteredQuestions
        {
            get
            {
                var filtered = allQuestions.AsEnumerable();

                // Search filter
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    var search = searchTerm.ToLowerInvariant();
                    filtered = filtered.Where(q =>
                        (q.Pytanie ?? "").ToLowerInvariant().Contains(search) ||
                        (q.OdpowiedzA ?? "").ToLowerInvariant().Contains(search) ||
                        (q.OdpowiedzB ?? "").ToLowerInvariant().Contains(search) ||
                        (q.OdpowiedzC ?? "").ToLowerInvariant().Contains(search) ||
                        (q.Kategorie ?? "").ToLowerInvariant().Contains(search)
                    );
                }

                // Category filter
                if (!string.IsNullOrWhiteSpace(selectedCategory))
                {
                    filtered = filtered.Where(q =>
                        (q.Kategorie ?? "").Contains(selectedCategory, StringComparison.OrdinalIgnoreCase)
                    );
                }

                return filtered;
            }
        }

        private IEnumerable<Question> FilteredAndSortedQuestions
        {
            get
            {
                var sorted = FilteredQuestions;

                sorted = sortMode switch
                {
                    SortMode.Newest => sorted.OrderByDescending(q => q.DataDodania).ThenByDescending(q => q.QuestionId),
                    SortMode.Oldest => sorted.OrderBy(q => q.DataDodania).ThenBy(q => q.QuestionId),
                    SortMode.QuestionAZ => sorted.OrderBy(q => q.Pytanie),
                    SortMode.QuestionZA => sorted.OrderByDescending(q => q.Pytanie),
                    SortMode.CategoryAZ => sorted.OrderBy(q => q.Kategorie),
                    _ => sorted
                };

                return sorted;
            }
        }

        private IEnumerable<Question> PaginatedQuestions
        {
            get
            {
                var skip = (currentPage - 1) * questionsPerPage;
                return FilteredAndSortedQuestions.Skip(skip).Take(questionsPerPage);
            }
        }

        private int TotalPages => (int)Math.Ceiling((double)FilteredAndSortedQuestions.Count() / questionsPerPage);

        protected override async Task OnInitializedAsync()
        {
            await LoadQuestions();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _selfRef = DotNetObjectReference.Create(this);
                try
                {
                    await JS.InvokeVoidAsync("modalKey.registerEsc", _selfRef, nameof(CloseMediaFromEsc));
                }
                catch { }
            }
        }

        private async Task LoadQuestions()
        {
            isLoading = true;
            loadFailed = false;
            StateHasChanged();

            try
            {
                var response = await HttpClient.GetAsync("/Question/GetQuestions");
                if (!response.IsSuccessStatusCode)
                {
                    loadFailed = true;
                    return;
                }

                var data = await response.Content.ReadFromJsonAsync<List<Question>>();
                if (data != null)
                {
                    allQuestions = data;
                    ExtractCategories();
                }
            }
            catch
            {
                loadFailed = true;
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private void ExtractCategories()
        {
            AvailableCategories.Clear();
            
            foreach (var question in allQuestions)
            {
                if (string.IsNullOrWhiteSpace(question.Kategorie)) continue;
                
                var categories = question.Kategorie.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var cat in categories)
                {
                    var trimmed = cat.Trim();
                    if (!string.IsNullOrEmpty(trimmed))
                    {
                        AvailableCategories.Add(trimmed);
                    }
                }
            }
        }

        private int GetCategoryCount(string category)
        {
            return allQuestions.Count(q => (q.Kategorie ?? "").Contains(category, StringComparison.OrdinalIgnoreCase));
        }

        private void SelectCategory(string? category)
        {
            selectedCategory = category;
            currentPage = 1;
            StateHasChanged();
        }

        private void SetViewMode(ViewMode mode)
        {
            viewMode = mode;
            StateHasChanged();
        }

        private void ClearSearch()
        {
            searchTerm = string.Empty;
            currentPage = 1;
            StateHasChanged();
        }

        private void ClearFilters()
        {
            searchTerm = string.Empty;
            selectedCategory = null;
            currentPage = 1;
            StateHasChanged();
        }

        private void ChangePage(int page)
        {
            if (page < 1 || page > TotalPages) return;
            currentPage = page;
            expandedQuestionId = null;
            StateHasChanged();
            
            // Scroll to top
            JS.InvokeVoidAsync("window.scrollTo", 0, 0);
        }

        private int GetPageStartIndex() => (currentPage - 1) * questionsPerPage + 1;
        private int GetPageEndIndex() => Math.Min(currentPage * questionsPerPage, FilteredAndSortedQuestions.Count());

        private List<int> GetVisiblePages()
        {
            var pages = new List<int>();
            const int maxVisible = 7;

            if (TotalPages <= maxVisible)
            {
                for (int i = 1; i <= TotalPages; i++)
                    pages.Add(i);
            }
            else
            {
                pages.Add(1);

                if (currentPage > 4)
                    pages.Add(-1); // ellipsis

                int start = Math.Max(2, currentPage - 2);
                int end = Math.Min(TotalPages - 1, currentPage + 2);

                for (int i = start; i <= end; i++)
                    pages.Add(i);

                if (currentPage < TotalPages - 3)
                    pages.Add(-1); // ellipsis

                pages.Add(TotalPages);
            }

            return pages;
        }

        private void ToggleQuestionDetails(int questionId)
        {
            expandedQuestionId = expandedQuestionId == questionId ? null : questionId;
            StateHasChanged();
        }

        private List<string> GetQuestionCategoriesList(Question q)
        {
            if (string.IsNullOrWhiteSpace(q.Kategorie)) return new List<string>();
            
            var parts = q.Kategorie.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var result = new List<string>();
            
            foreach (var p in parts)
            {
                var trimmed = p.Trim();
                if (!string.IsNullOrEmpty(trimmed) && seen.Add(trimmed))
                {
                    result.Add(trimmed);
                }
            }
            
            return result;
        }

        // Helper to check if question is Yes/No type
        private static bool IsYesNoQuestion(Question q)
        {
            return string.IsNullOrWhiteSpace(q.OdpowiedzA) && 
                   string.IsNullOrWhiteSpace(q.OdpowiedzB) && 
                   string.IsNullOrWhiteSpace(q.OdpowiedzC);
        }

        // Get all answers for the question (handles both ABC and Yes/No types)
        private static List<(string Letter, string Text)> GetQuestionAnswers(Question q)
        {
            var answers = new List<(string Letter, string Text)>();
            
            if (IsYesNoQuestion(q))
            {
                answers.Add(("Tak", "Tak"));
                answers.Add(("Nie", "Nie"));
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(q.OdpowiedzA))
                    answers.Add(("A", q.OdpowiedzA));
                    
                if (!string.IsNullOrWhiteSpace(q.OdpowiedzB))
                    answers.Add(("B", q.OdpowiedzB));
                    
                if (!string.IsNullOrWhiteSpace(q.OdpowiedzC))
                    answers.Add(("C", q.OdpowiedzC));
            }
            
            return answers;
        }

        private static string GetCorrectAnswerText(Question q)
        {
            if (IsYesNoQuestion(q))
            {
                // For Yes/No questions, the answer is directly in PoprawnaOdp
                var answer = (q.PoprawnaOdp ?? "").Trim();
                return answer.Equals("Tak", StringComparison.OrdinalIgnoreCase) ? "Tak" : 
                       answer.Equals("Nie", StringComparison.OrdinalIgnoreCase) ? "Nie" : 
                       answer;
            }
            
            // For ABC questions
            var letter = (q.PoprawnaOdp ?? "").Trim().ToUpperInvariant();
            return letter switch
            {
                "A" => q.OdpowiedzA ?? "",
                "B" => q.OdpowiedzB ?? "",
                "C" => q.OdpowiedzC ?? "",
                _ => q.PoprawnaOdp ?? ""
            };
        }

        // Check if specific answer is correct
        private static bool IsAnswerCorrect(Question q, string answerLetter)
        {
            if (IsYesNoQuestion(q))
            {
                return (q.PoprawnaOdp ?? "").Trim().Equals(answerLetter, StringComparison.OrdinalIgnoreCase);
            }
            
            return (q.PoprawnaOdp ?? "").Trim().Equals(answerLetter, StringComparison.OrdinalIgnoreCase);
        }

        private string GetCategoryIcon(string category)
        {
            // Match category to icon
            return category.ToUpperInvariant() switch
            {
                "AM" => "bi bi-scooter",
                "A1" or "A2" or "A" => "bi bi-bicycle",
                "B1" or "B" or "B+E" => "bi bi-car-front-fill",
                "C1" or "C" or "C1+E" or "C+E" => "bi bi-truck-front",
                "D1" or "D" or "D1+E" or "D+E" => "bi bi-bus-front",
                "T" => "bi bi-truck-flatbed",
                "PT" => "bi bi-train-front",
                _ => "bi bi-card-checklist"
            };
        }

        private string GetMediaIcon(string? mediaFileName)
        {
            if (string.IsNullOrWhiteSpace(mediaFileName)) return "bi bi-file-earmark";
            
            var extension = System.IO.Path.GetExtension(mediaFileName).ToLowerInvariant();
            
            return extension switch
            {
                ".mp4" or ".avi" or ".mov" or ".wmv" or ".webm" => "bi bi-camera-video-fill",
                ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" or ".webp" => "bi bi-image-fill",
                ".pdf" => "bi bi-file-pdf-fill",
                _ => "bi bi-file-earmark-play"
            };
        }

        private async Task OpenMedia(Question q)
        {
            if (q == null || string.IsNullOrWhiteSpace(q.Media)) return;
            
            try
            {
                currentMediaQuestion = q;
                
                if (_selfRef == null) _selfRef = DotNetObjectReference.Create(this);
                try { await JS.InvokeVoidAsync("modalKey.registerEsc", _selfRef, nameof(CloseMediaFromEsc)); } catch { }

                var resp = await HttpClient.GetAsync($"/QuestionFile/GetQuestionFileByName?questionFileName={q.Media}");
                if (!resp.IsSuccessStatusCode) 
                {
                    currentMediaQuestion = null;
                    return;
                }
                
                var file = await resp.Content.ReadFromJsonAsync<QuestionFile>();
                if (file?.File == null || string.IsNullOrWhiteSpace(file.ContentType)) 
                {
                    currentMediaQuestion = null;
                    return;
                }

                mediaUrl = await JS.InvokeAsync<string>("createObjectURL", file.File, file.ContentType);
                mediaContentType = file.ContentType;
                isMediaOpen = true;
                StateHasChanged();
            }
            catch 
            {
                currentMediaQuestion = null;
            }
        }

        [JSInvokable]
        public async Task CloseMediaFromEsc()
        {
            if (isMediaOpen)
            {
                await CloseMedia();
                StateHasChanged();
            }
        }

        private async Task CloseMedia()
        {
            try
            {
                if (!string.IsNullOrEmpty(mediaUrl))
                {
                    await JS.InvokeVoidAsync("revokeObjectURL", mediaUrl);
                }
            }
            catch { }
            finally
            {
                mediaUrl = null;
                mediaContentType = null;
                currentMediaQuestion = null;
                isMediaOpen = false;
            }
        }

        private string GetMediaTypeName(string? contentType)
        {
            if (string.IsNullOrWhiteSpace(contentType)) return "Nieznany";
            
            return contentType.ToLowerInvariant() switch
            {
                var ct when ct.StartsWith("video/") => "Wideo",
                var ct when ct.StartsWith("image/") => "Obraz",
                var ct when ct.StartsWith("application/pdf") => "PDF",
                _ => "Plik"
            };
        }

        private void ReloadPage()
        {
            Navigation.NavigateTo(Navigation.Uri, forceLoad: true);
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                await JS.InvokeVoidAsync("modalKey.unregisterEsc");
            }
            catch { }
            _selfRef?.Dispose();
        }
    }

    public enum SortMode
    {
        Newest,
        Oldest,
        QuestionAZ,
        QuestionZA,
        CategoryAZ
    }

    public enum ViewMode
    {
        Cards,
        List,
        Compact
    }
}
