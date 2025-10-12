using DriverGuide.Domain.Enums;
using DriverGuide.Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

namespace DriverGuide.UI.Pages.Quiz
{
    public partial class Quiz
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        private AuthenticationStateProvider AuthStateProvider { get; set; } = default!;

        [Inject]
        private HttpClient Http { get; set; } = default!;

        [Parameter]
        public string? Category { get; set; }

        private double questionsCount => questions?.Count ?? 32;

        private string? fileBlobUrl;
        private int currentIndex = 0;
        private string? selectedAnswer;
        private bool isVideoLoading = true;
        private bool isLoadingQuestion = false;

        // Dodane flagi i kolekcje dla optymalizacji
        private bool isUserAuthenticated = false;
        private string? testSessionId;
        private readonly List<StoredQuestionAnswer> storedAnswers = [];
        private DateTimeOffset testStartTime;

        private Question? currentQuestion;
        private readonly List<Question>? questions = [];
        private readonly List<QuestionFile>? questionsFiles = [];

        protected override async Task OnInitializedAsync()
        {
            isLoadingQuestion = true;

            try
            {
                // Spróbuj sprawdzić stan uwierzytelnienia
                var authState = await AuthStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                isUserAuthenticated = user.Identity?.IsAuthenticated ?? false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking authentication state: {ex.Message}");
                isUserAuthenticated = false;
            }

            // Zapisz czas rozpoczęcia testu
            testStartTime = DateTimeOffset.Now;

            // Dla zalogowanego użytkownika - utwórz sesję testu w bazie
            if (isUserAuthenticated)
            {
                await InitializeTestSessionInDatabase();
            }

            try
            {
                using var questionsHttpResponse = await Http.GetAsync($"/Question/GetQuizQuestions?category={Category}");

                var status = (int)questionsHttpResponse.StatusCode;
                var contentType = questionsHttpResponse.Content.Headers.ContentType?.MediaType;

                if (!questionsHttpResponse.IsSuccessStatusCode)
                {
                    var raw = await questionsHttpResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Request failed: {status} {questionsHttpResponse.StatusCode}, Content-Type={contentType}, Snippet={raw[..Math.Min(raw.Length, 200)]}");
                    return;
                }

                // Guard: if HTML, abort JSON parse early
                if (contentType != null && contentType.Contains("html", StringComparison.OrdinalIgnoreCase))
                {
                    var raw = await questionsHttpResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Unexpected HTML instead of JSON. Status={status}. Snippet={raw[..Math.Min(raw.Length, 200)]}");
                    return;
                }

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var questionsResponse = await questionsHttpResponse.Content.ReadFromJsonAsync<List<Question>>(options);

                if (questionsResponse is null || questionsResponse.Count == 0)
                {
                    Console.WriteLine("Questions response empty or null.");
                    return;
                }

                questions!.AddRange(questionsResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fetch questions threw: {ex}");
            }

            await LoadQuestionAsync();
        }

        private async Task InitializeTestSessionInDatabase()
        {
            try
            {
                var categoryEnum = Enum.Parse<LicenseCategory>(Category ?? "B");
                var userId = await GetCurrentUserId();

                var response = await Http.PostAsJsonAsync("/TestSession/Create",
                    new { Category = categoryEnum, UserId = userId });

                if (response.IsSuccessStatusCode)
                {
                    testSessionId = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Created test session: {testSessionId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing test session: {ex.Message}");
            }
        }

        private async Task<Guid?> GetCurrentUserId()
        {
            if (!isUserAuthenticated) return null;

            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return userId;
            }

            return null;
        }

        private async Task LoadQuestionAsync()
        {
            if (questions is null || currentIndex >= questions.Count)
                return;

            isLoadingQuestion = true;
            StateHasChanged();

            selectedAnswer = null;
            currentQuestion = questions[currentIndex];

            // Dla zalogowanego użytkownika - zacznij śledzić pytanie w bazie
            if (isUserAuthenticated && !string.IsNullOrEmpty(testSessionId))
            {
                await RegisterQuestionStartInDatabase();
            }

            // Wyczyść poprzedni URL bloba
            if (!string.IsNullOrWhiteSpace(fileBlobUrl))
            {
                try { await JS.InvokeVoidAsync("revokeObjectURL", fileBlobUrl); }
                catch { /* Ignoruj błędy przy czyszczeniu */ }
                fileBlobUrl = null;
            }

            // Załaduj media dla pytania, jeśli istnieje
            if (!string.IsNullOrWhiteSpace(currentQuestion.Media))
            {
                await LoadMediaForCurrentQuestion();
            }

            await Task.Delay(300);
            isLoadingQuestion = false;
            StateHasChanged();
        }

        private async Task RegisterQuestionStartInDatabase()
        {
            if (currentQuestion == null || string.IsNullOrEmpty(testSessionId))
                return;

            try
            {
                var categoryEnum = Enum.Parse<LicenseCategory>(Category ?? "B");

                await Http.PostAsJsonAsync("/QuestionAnswer/StartQuestion",
                    new
                    {
                        TestSessionId = testSessionId,
                        QuestionId = currentQuestion.QuestionId.ToString(),
                        QuestionCategory = categoryEnum,
                        Question = currentQuestion.Pytanie,
                        CorrectQuestionAnswer = currentQuestion.PoprawnaOdp,
                        StartDate = DateTimeOffset.Now,
                        QuestionLanguage = Language.PL
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering question start: {ex.Message}");
            }
        }

        private async Task LoadMediaForCurrentQuestion()
        {
            // Implementacja bez zmian
            if (currentQuestion == null || string.IsNullOrWhiteSpace(currentQuestion.Media))
                return;

            var cachedFile = questionsFiles?.Find(x => x.Name == currentQuestion.Media);
            if (cachedFile != null)
            {
                await CreateBlobUrl(cachedFile);
                return;
            }

            var fileResponse = await Http.GetAsync($"/QuestionFile/GetQuestionFileByName?questionFileName={currentQuestion.Media}");
            if (!fileResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to fetch media file: {fileResponse.StatusCode}");
                return;
            }

            var questionFile = await fileResponse.Content.ReadFromJsonAsync<QuestionFile>();
            if (questionFile is null)
            {
                Console.WriteLine("QuestionFile deserialized as null");
                return;
            }

            if (questionFile.File == null || questionFile.File.Length == 0)
            {
                Console.WriteLine("QuestionFile has no data");
                return;
            }

            questionsFiles!.Add(questionFile);
            await CreateBlobUrl(questionFile);
        }

        private async Task CreateBlobUrl(QuestionFile file)
        {
            // Implementacja bez zmian
            if (file?.File == null || string.IsNullOrWhiteSpace(file.ContentType))
            {
                Console.WriteLine("Cannot create blob URL: missing file data or content type");
                return;
            }

            try
            {
                fileBlobUrl = await JS.InvokeAsync<string>("createObjectURL", file.File, file.ContentType);
                Console.WriteLine($"Successfully created blob URL for {file.Name} ({file.ContentType})");

                var mimeType = !string.IsNullOrEmpty(currentQuestion?.Media) ?
                    Utils.FileExtensionUtils.GetMimeType(currentQuestion.Media) : string.Empty;

                if (mimeType.StartsWith("video/"))
                {
                    await Task.Delay(200);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating blob URL: {ex.Message}");
                fileBlobUrl = null;
            }
        }

        private void SelectAnswer(string answer)
        {
            selectedAnswer = answer;
            StateHasChanged();
        }

        private async Task SubmitAnswer()
        {
            if (selectedAnswer == null || currentQuestion == null || isLoadingQuestion)
                return;

            var isLastQuestion = currentIndex == (questions?.Count ?? 0) - 1;

            if (isUserAuthenticated)
            {
                // Zalogowany użytkownik - zapisz odpowiedź natychmiast
                await RegisterAnswerInDatabase();

                if (isLastQuestion)
                {
                    await CompleteTestInDatabase();
                    NavigateToSummary();
                }
                else
                {
                    await NextQuestion();
                }
            }
            else
            {
                // Niezalogowany użytkownik - przechowaj odpowiedź lokalnie
                StoreAnswerLocally();

                if (isLastQuestion)
                {
                    // Ostatnie pytanie - zapisz cały test do bazy
                    await SaveEntireTestToDatabase();
                    NavigateToSummary();
                }
                else
                {
                    await NextQuestion();
                }
            }
        }

        private async Task RegisterAnswerInDatabase()
        {
            if (currentQuestion == null || string.IsNullOrEmpty(testSessionId) || string.IsNullOrEmpty(selectedAnswer))
                return;

            try
            {
                await Http.PostAsJsonAsync("/QuestionAnswer/SubmitAnswer",
                    new
                    {
                        TestSessionId = testSessionId,
                        QuestionId = currentQuestion.QuestionId.ToString(),
                        UserAnswer = selectedAnswer,
                        EndDate = DateTimeOffset.Now,
                        StartDate = testStartTime,
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering answer: {ex.Message}");
            }
        }

        private void StoreAnswerLocally()
        {
            if (currentQuestion == null || string.IsNullOrEmpty(selectedAnswer))
                return;

            // Przechowaj odpowiedź lokalnie
            storedAnswers.Add(new StoredQuestionAnswer
            {
                QuestionId = currentQuestion.QuestionId.ToString(),
                QuestionCategory = Enum.Parse<LicenseCategory>(Category ?? "B"),
                Question = currentQuestion.Pytanie ?? string.Empty,
                CorrectQuestionAnswer = currentQuestion.PoprawnaOdp ?? string.Empty,
                UserQuestionAnswer = selectedAnswer,
                StartDate = DateTimeOffset.Now.AddSeconds(-5), // Przybliżony czas rozpoczęcia
                EndDate = DateTimeOffset.Now,
                QuestionLanguage = Language.PL
            });
        }

        private async Task CompleteTestInDatabase()
        {
            if (string.IsNullOrEmpty(testSessionId))
                return;

            try
            {
                // Oblicz wynik testu
                double correctAnswersCount = 0;

                foreach (var answer in storedAnswers)
                {
                    if (answer.UserQuestionAnswer == answer.CorrectQuestionAnswer)
                    {
                        correctAnswersCount++;
                    }
                }

                var result = (correctAnswersCount / questions!.Count) * 100;

                await Http.PostAsJsonAsync("/TestSession/Complete",
                    new { TestSessionId = testSessionId, Result = result });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error completing test: {ex.Message}");
            }
        }

        private async Task SaveEntireTestToDatabase()
        {
            try
            {
                // 1. Utwórz sesję testu
                var categoryEnum = Enum.Parse<LicenseCategory>(Category ?? "B");
                var createResponse = await Http.PostAsJsonAsync("/TestSession/Create", new 
                { 
                        Category = categoryEnum, 
                        StartDate = testStartTime 
                });

                if (!createResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("Failed to create test session");
                    return;
                }

                testSessionId = await createResponse.Content.ReadAsStringAsync();

                // 2. Zapisz wszystkie odpowiedzi za jednym razem
                await Http.PostAsJsonAsync("/QuestionAnswer/BulkSubmitAnswers",
                    new
                    {
                        TestSessionId = testSessionId,
                        Answers = storedAnswers.Select(a => new {
                            QuestionId = a.QuestionId,
                            QuestionCategory = a.QuestionCategory,
                            Question = a.Question,
                            CorrectQuestionAnswer = a.CorrectQuestionAnswer,
                            UserQuestionAnswer = a.UserQuestionAnswer,
                            StartDate = a.StartDate,
                            EndDate = a.EndDate,
                            QuestionLanguage = a.QuestionLanguage
                        }).ToList()
                    });

                // 3. Zakończ test z wynikiem
                double correctAnswersCount = storedAnswers.Count(a => a.UserQuestionAnswer == a.CorrectQuestionAnswer);
                double result = (correctAnswersCount / questions!.Count) * 100;

                await Http.PostAsJsonAsync("/TestSession/Complete",
                    new { TestSessionId = testSessionId, Result = result });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving test to database: {ex.Message}");
            }
        }

        private void NavigateToSummary()
        {
            if (!string.IsNullOrEmpty(testSessionId))
            {
                NavigationManager.NavigateTo($"/test-summary/{testSessionId}");
            }
            else
            {
                NavigationManager.NavigateTo("/");
            }
        }

        private async Task NextQuestion()
        {
            currentIndex++;
            await LoadQuestionAsync();
        }

        private IEnumerable<string> GetCurrentQuestionAnswers()
        {
            List<string> result = new();
            if (questions is null || currentQuestion is null)
                return result;

            if (string.IsNullOrWhiteSpace(currentQuestion.OdpowiedzA) && string.IsNullOrWhiteSpace(currentQuestion.OdpowiedzB) && string.IsNullOrWhiteSpace(currentQuestion.OdpowiedzC))
                result.AddRange(new[] { "Tak", "Nie" });
            else
                result.AddRange(new[] { currentQuestion.OdpowiedzA!, currentQuestion.OdpowiedzB!, currentQuestion.OdpowiedzC! });

            return result;
        }

        private void OnVideoLoadStart()
        {
            isVideoLoading = true;
            StateHasChanged();
        }

        private void OnVideoCanPlay()
        {
            isVideoLoading = false;
            StateHasChanged();
        }

        private void OnVideoError()
        {
            isVideoLoading = false;
            Console.WriteLine("Video error occurred");
            StateHasChanged();
        }
    }

    // Klasa do przechowywania odpowiedzi lokalnie
    public class StoredQuestionAnswer
    {
        public string QuestionId { get; set; } = string.Empty;
        public LicenseCategory QuestionCategory { get; set; }
        public string Question { get; set; } = string.Empty;
        public string CorrectQuestionAnswer { get; set; } = string.Empty;
        public string UserQuestionAnswer { get; set; } = string.Empty;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public Language QuestionLanguage { get; set; } = Language.PL;
    }
}