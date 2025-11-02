using DriverGuide.Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace DriverGuide.UI.Pages.Questions
{
    public partial class Questions : ComponentBase, IAsyncDisposable
    {
        [Inject] private HttpClient HttpClient { get; set; } = default!;
        [Inject] private IJSRuntime JS { get; set; } = default!;

        private DotNetObjectReference<Questions>? _selfRef;

        private List<Question> allQuestions = new();
        private string searchTerm = string.Empty;
        private bool isLoading = true;
        private bool loadFailed = false;

        // Modal state for media preview
        private bool isMediaOpen = false;
        private string? mediaUrl;
        private string? mediaContentType;

        protected override async Task OnInitializedAsync()
        {
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
                    allQuestions = data;
            }
            catch
            {
                loadFailed = true;
            }
            finally
            {
                isLoading = false;
            }
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
                catch
                {
                    // Script might not be ready yet (hot reload/cache). Ignore to avoid crash.
                }
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

        private IEnumerable<Question> FilteredQuestions =>
        string.IsNullOrWhiteSpace(searchTerm)
        ? allQuestions
        : allQuestions.Where(q =>
        (q.Pytanie ?? string.Empty).Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
        (q.OdpowiedzA ?? string.Empty).Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
        (q.OdpowiedzB ?? string.Empty).Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
        (q.OdpowiedzC ?? string.Empty).Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
        (q.Kategorie ?? string.Empty).Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
        );

        private static string GetCorrectAnswer(Question q) => q.PoprawnaOdp ?? string.Empty;
        private static string GetCorrectAnswerText(Question q)
        {
            var letter = (q.PoprawnaOdp ?? string.Empty).Trim();
            return letter.ToUpperInvariant() switch
            {
                "A" => q.OdpowiedzA ?? string.Empty,
                "B" => q.OdpowiedzB ?? string.Empty,
                "C" => q.OdpowiedzC ?? string.Empty,
                _ => q.PoprawnaOdp ?? string.Empty
            };
        }

        private static string GetCategoriesText(Question q)
        {
            var raw = q.Kategorie ?? string.Empty;
            if (string.IsNullOrWhiteSpace(raw)) return string.Empty;
            // Normalize: split by comma, trim spaces, dedupe, keep order
            var parts = raw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var result = new List<string>();
            foreach (var p in parts)
            {
                var up = p.Trim();
                if (up.Length == 0) continue;
                if (seen.Add(up)) result.Add(up);
            }
            return string.Join(", ", result);
        }

        private async Task OpenMedia(Question q)
        {
            if (q == null || string.IsNullOrWhiteSpace(q.Media)) return;
            try
            {
                // Ensure ESC registered (idempotent on JS side)
                if (_selfRef == null) _selfRef = DotNetObjectReference.Create(this);
                try { await JS.InvokeVoidAsync("modalKey.registerEsc", _selfRef, nameof(CloseMediaFromEsc)); } catch { }

                var resp = await HttpClient.GetAsync($"/QuestionFile/GetQuestionFileByName?questionFileName={q.Media}");
                if (!resp.IsSuccessStatusCode) return;
                var file = await resp.Content.ReadFromJsonAsync<QuestionFile>();
                if (file?.File == null || string.IsNullOrWhiteSpace(file.ContentType)) return;

                mediaUrl = await JS.InvokeAsync<string>("createObjectURL", file.File, file.ContentType);
                mediaContentType = file.ContentType;
                isMediaOpen = true;
                StateHasChanged();
            }
            catch { /* ignore */ }
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
            catch { /* ignore */ }
            finally
            {
                mediaUrl = null;
                mediaContentType = null;
                isMediaOpen = false;
            }
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                await JS.InvokeVoidAsync("modalKey.unregisterEsc");
            }
            catch { /* ignore */ }
            _selfRef?.Dispose();
        }
    }
}
