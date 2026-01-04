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
    public partial class Quiz : IDisposable
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        private AuthenticationStateProvider AuthStateProvider { get; set; } = default!;

        [Inject]
        private HttpClient Http { get; set; } = default!;

        [Parameter]
        public string? Category { get; set; }

        [Parameter]
        public string? TestSessionId { get; set; }

        private double questionsCount => questions?.Count ?? 32;

        private string? fileBlobUrl;
        private int currentIndex = 0;
        private string? selectedAnswer;
        private bool isVideoLoading = true;
        private bool isLoadingQuestion = false;

        // Timer variables
        private System.Threading.Timer? questionTimer;
        private int remainingTimeSeconds = 0;
        private bool isTimerRunning = false;

        // Dodane flagi i kolekcje dla optymalizacji
        private bool isUserAuthenticated = false;
        private string? testSessionId;
        private readonly List<StoredQuestionAnswer> storedAnswers = [];
        private DateTimeOffset testStartTime;
        private bool isContinuingTest = false;
        private HashSet<string> answeredQuestionIds = new();

        private Question? currentQuestion;
        private readonly List<Question>? questions = [];
        private readonly List<QuestionFile>? questionsFiles = [];

        protected override async Task OnInitializedAsync()
        {
            isLoadingQuestion = true;

            try
            {
                // Spróbuj sprawdziæ stan uwierzytelnienia
                var authState = await AuthStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                isUserAuthenticated = user.Identity?.IsAuthenticated ?? false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking authentication state: {ex.Message}");
                isUserAuthenticated = false;
            }

            // Zapisz czas rozpoczêcia testu
            testStartTime = DateTimeOffset.Now;

            // SprawdŸ czy kontynuujemy test czy rozpoczynamy nowy
            bool isNewTest = string.IsNullOrEmpty(TestSessionId);
            
            if (!isNewTest)
            {
                isContinuingTest = true;
                testSessionId = TestSessionId;
                Console.WriteLine($"Continuing test session: {testSessionId}");
                
                // Dla kontynuacji: najpierw za³aduj odpowiedzi, potem pytania w oryginalnej kolejnoœci
                await LoadExistingTestSession();
                await LoadQuestionsForContinuation();
            }
            else
            {
                // Dla nowego testu: za³aduj losowe pytania
                await LoadRandomQuestions();
                
                // Dla zalogowanego u¿ytkownika - utwórz sesjê testu w bazie
                if (isUserAuthenticated)
                {
                    await InitializeTestSessionInDatabase();
                }
            }

            // Za³aduj pierwsze pytanie
            await LoadQuestionAsync();
        }

        private async Task LoadRandomQuestions()
        {
            Console.WriteLine("=== LoadRandomQuestions START ===");
            
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
                Console.WriteLine($"? Loaded {questions.Count} random questions for category {Category}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Fetch questions threw: {ex}");
            }
            
            Console.WriteLine("=== LoadRandomQuestions END ===");
        }

        private async Task LoadQuestionsForContinuation()
        {
            Console.WriteLine("=== LoadQuestionsForContinuation START ===");
            
            try
            {
                using var allQuestionsResponse = await Http.GetAsync($"/Question/GetQuizQuestions?category={Category}");
                
                if (!allQuestionsResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine($"? Failed to load questions: {allQuestionsResponse.StatusCode}");
                    return;
                }

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var allQuestions = await allQuestionsResponse.Content.ReadFromJsonAsync<List<Question>>(options);

                if (allQuestions is null || allQuestions.Count == 0)
                {
                    Console.WriteLine("? No questions available");
                    return;
                }

                Console.WriteLine($"Available questions in database: {allQuestions.Count}");

                var answersResponse = await Http.GetAsync($"/QuestionAnswer/GetByTestSession/{testSessionId}");
                
                if (!answersResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("? Failed to load session answers");
                    return;
                }

                var sessionAnswers = await answersResponse.Content.ReadFromJsonAsync<List<QuestionAnswer>>(options);
                
                if (sessionAnswers is null || !sessionAnswers.Any())
                {
                    Console.WriteLine("? No answers in session, loading random questions");
                    await LoadRandomQuestions();
                    return;
                }

                var orderedAnswers = sessionAnswers.OrderBy(a => a.StartDate).ToList();
                Console.WriteLine($"Session has {orderedAnswers.Count} questions (in original order)");

                var questionIds = orderedAnswers.Select(a => a.QuestionId).ToList();
                var sessionQuestions = new List<Question>();

                foreach (var questionId in questionIds)
                {
                    var question = allQuestions.FirstOrDefault(q => q.QuestionId == questionId);
                    if (question != null)
                    {
                        sessionQuestions.Add(question);
                        Console.WriteLine($"  ? Added question ID {questionId} to position {sessionQuestions.Count}");
                    }
                    else
                    {
                        Console.WriteLine($"  ? Question ID {questionId} not found in database!");
                    }
                }

                var usedQuestionIds = questionIds.ToHashSet();
                var remainingQuestions = allQuestions
                    .Where(q => !usedQuestionIds.Contains(q.QuestionId))
                    .ToList();

                var random = new Random();
                var questionsNeeded = 32 - sessionQuestions.Count;
                
                if (questionsNeeded > 0 && remainingQuestions.Any())
                {
                    var additionalQuestions = remainingQuestions
                        .OrderBy(x => random.Next())
                        .Take(questionsNeeded)
                        .ToList();
                    
                    sessionQuestions.AddRange(additionalQuestions);
                    Console.WriteLine($"? Added {additionalQuestions.Count} random questions to complete the test");
                }

                questions!.AddRange(sessionQuestions);
                Console.WriteLine($"? Total questions loaded: {questions.Count}");

                FindNextUnansweredQuestion();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Error loading questions for continuation: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                await LoadRandomQuestions();
            }
            
            Console.WriteLine("=== LoadQuestionsForContinuation END ===");
        }

        private async Task LoadExistingTestSession()
        {
            Console.WriteLine($"=== LoadExistingTestSession START ===");
            Console.WriteLine($"Loading answers for testSessionId: {testSessionId}");
            
            try
            {
                var answersResponse = await Http.GetAsync($"/QuestionAnswer/GetByTestSession/{testSessionId}");

                Console.WriteLine($"Response status: {answersResponse.StatusCode}");

                if (answersResponse.IsSuccessStatusCode)
                {
                    var answers = await answersResponse.Content.ReadFromJsonAsync<List<QuestionAnswer>>();

                    Console.WriteLine($"Total answers received: {answers?.Count ?? 0}");

                    if (answers != null && answers.Any())
                    {
                        // Zapisz ID pytañ, które ju¿ maj¹ odpowiedŸ
                        answeredQuestionIds = answers
                            .Where(a => !string.IsNullOrEmpty(a.UserQuestionAnswer))
                            .Select(a => a.QuestionId.ToString())
                            .ToHashSet();

                        Console.WriteLine($"? Loaded {answeredQuestionIds.Count} answered questions from session {testSessionId}");
                        
                        // Wyœwietl szczegó³y odpowiedzianych pytañ
                        foreach (var answer in answers.Where(a => !string.IsNullOrEmpty(a.UserQuestionAnswer)))
                        {
                            Console.WriteLine($"  - QuestionId: {answer.QuestionId}, Answer: {answer.UserQuestionAnswer}");
                        }
                        
                        // Wyœwietl pytania bez odpowiedzi (jeœli s¹)
                        var unanswered = answers.Where(a => string.IsNullOrEmpty(a.UserQuestionAnswer)).ToList();
                        if (unanswered.Any())
                        {
                            Console.WriteLine($"? Found {unanswered.Count} questions WITHOUT answers (started but not answered):");
                            foreach (var answer in unanswered)
                            {
                                Console.WriteLine($"  - QuestionId: {answer.QuestionId}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("? No answers found for this session");
                    }
                }
                else
                {
                    Console.WriteLine($"? Failed to load answers: {answersResponse.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Error loading existing test session: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
            
            Console.WriteLine($"=== LoadExistingTestSession END ===");
        }

        private void FindNextUnansweredQuestion()
        {
            Console.WriteLine($"=== FindNextUnansweredQuestion START ===");
            Console.WriteLine($"Total questions loaded: {questions!.Count}");
            Console.WriteLine($"Answered questions count: {answeredQuestionIds.Count}");
            Console.WriteLine($"Answered question IDs: {string.Join(", ", answeredQuestionIds)}");
            
            // ZnajdŸ pierwsze pytanie bez odpowiedzi
            for (int i = 0; i < questions!.Count; i++)
            {
                var questionId = questions[i].QuestionId.ToString();
                var isAnswered = answeredQuestionIds.Contains(questionId);
                
                Console.WriteLine($"Question {i + 1}: ID={questionId}, IsAnswered={isAnswered}");
                
                if (!isAnswered)
                {
                    currentIndex = i;
                    Console.WriteLine($"? Found unanswered question at index {currentIndex} (question {currentIndex + 1})");
                    Console.WriteLine($"=== FindNextUnansweredQuestion END ===");
                    return;
                }
            }

            // Jeœli wszystkie pytania maj¹ odpowiedzi, u¿ytkownik powinien byæ przekierowany przy ³adowaniu
            Console.WriteLine($"? All {answeredQuestionIds.Count} questions have been answered");
            currentIndex = questions.Count; // Ustawienie poza zakresem aby nie pokazywaæ pytañ
            Console.WriteLine($"=== FindNextUnansweredQuestion END ===");
        }

        private async Task InitializeTestSessionInDatabase()
        {
            try
            {
                var categoryEnum = Enum.Parse<LicenseCategory>(Category ?? "B");
                var userId = await GetCurrentUserId();

                var response = await Http.PostAsJsonAsync("/TestSession/Create",
                    new { Category = categoryEnum, StartDate = testStartTime, UserId = userId });

                if (response.IsSuccessStatusCode)
                {
                    testSessionId = await response.Content.ReadAsStringAsync();
                    // Usuñ cudzys³owy z ID jeœli istniej¹
                    testSessionId = testSessionId?.Trim('"');
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
            {
                Console.WriteLine($"Cannot load question: currentIndex={currentIndex}, questionsCount={questions?.Count ?? 0}");
                return;
            }

            isLoadingQuestion = true;
            StateHasChanged();

            // Stop previous timer if exists
            StopTimer();

            selectedAnswer = null;
            currentQuestion = questions[currentIndex];

            Console.WriteLine($"Loading question {currentIndex + 1}/{questions.Count}: {currentQuestion.QuestionId}");

            // Dla zalogowanego u¿ytkownika rozpoczynaj¹cego nowy test - zacznij œledziæ pytanie w bazie
            if (isUserAuthenticated && !string.IsNullOrEmpty(testSessionId) && !isContinuingTest)
            {
                await RegisterQuestionStartInDatabase();
            }

            // Dla kontynuowanego testu - zarejestruj pytanie jeœli jeszcze nie by³o
            if (isContinuingTest && isUserAuthenticated && !string.IsNullOrEmpty(testSessionId))
            {
                var questionId = currentQuestion.QuestionId.ToString();
                if (!answeredQuestionIds.Contains(questionId))
                {
                    await RegisterQuestionStartInDatabase();
                }
            }

            // Wyczyœæ poprzedni URL bloba
            if (!string.IsNullOrWhiteSpace(fileBlobUrl))
            {
                try { await JS.InvokeVoidAsync("revokeObjectURL", fileBlobUrl); }
                catch { /* Ignoruj b³êdy przy czyszczeniu */ }
                fileBlobUrl = null;
            }

            // Za³aduj media dla pytania, jeœli istnieje
            if (!string.IsNullOrWhiteSpace(currentQuestion.Media))
            {
                await LoadMediaForCurrentQuestion();
            }

            await Task.Delay(300);
            isLoadingQuestion = false;
            
            // Start timer for this question
            StartTimer(currentQuestion.TimeToAnswerSeconds);
            
            StateHasChanged();
        }

        private void StartTimer(int seconds)
        {
            remainingTimeSeconds = seconds;
            isTimerRunning = true;
            
            questionTimer = new System.Threading.Timer(async _ =>
            {
                if (remainingTimeSeconds > 0)
                {
                    remainingTimeSeconds--;
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    // Time's up - auto submit with no answer (incorrect)
                    isTimerRunning = false;
                    await InvokeAsync(async () => await HandleTimeExpired());
                }
            }, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }

        private void StopTimer()
        {
            isTimerRunning = false;
            questionTimer?.Dispose();
            questionTimer = null;
        }

        private async Task HandleTimeExpired()
        {
            Console.WriteLine($"Time expired for question {currentQuestion?.QuestionId}");
            
            if (currentQuestion == null)
                return;

            var questionId = currentQuestion.QuestionId.ToString();
            answeredQuestionIds.Add(questionId);

            var isLastQuestion = currentIndex == (questions?.Count ?? 0) - 1;

            if (isUserAuthenticated && !string.IsNullOrEmpty(testSessionId))
            {
                // Zapisz pust¹ odpowiedŸ (niepoprawn¹) do bazy
                await RegisterTimeoutAnswerInDatabase();

                var allQuestionsAnswered = answeredQuestionIds.Count >= questions!.Count;
                
                if (isLastQuestion || allQuestionsAnswered)
                {
                    Console.WriteLine($"Completing test after timeout: isLastQuestion={isLastQuestion}, allAnswered={allQuestionsAnswered}");
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
                // Dla niezalogowanego - zapisz lokalnie pust¹ odpowiedŸ
                StoreTimeoutAnswerLocally();

                if (isLastQuestion)
                {
                    await SaveEntireTestToDatabase();
                    NavigateToSummary();
                }
                else
                {
                    await NextQuestion();
                }
            }
        }

        private async Task RegisterTimeoutAnswerInDatabase()
        {
            if (currentQuestion == null || string.IsNullOrEmpty(testSessionId))
                return;

            try
            {
                // Zapisz pust¹ odpowiedŸ jako timeout
                await Http.PostAsJsonAsync("/QuestionAnswer/SubmitAnswer",
                    new
                    {
                        TestSessionId = testSessionId,
                        QuestionId = currentQuestion.QuestionId,
                        UserAnswer = "", // Pusta odpowiedŸ oznacza timeout
                        EndDate = DateTimeOffset.Now,
                        StartDate = testStartTime,
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering timeout answer: {ex.Message}");
            }
        }

        private void StoreTimeoutAnswerLocally()
        {
            if (currentQuestion == null)
                return;

            storedAnswers.Add(new StoredQuestionAnswer
            {
                QuestionId = currentQuestion.QuestionId,
                QuestionCategory = Enum.Parse<LicenseCategory>(Category ?? "B"),
                Question = currentQuestion.Pytanie ?? string.Empty,
                CorrectQuestionAnswer = currentQuestion.PoprawnaOdp ?? string.Empty,
                UserQuestionAnswer = "", // Pusta odpowiedŸ
                StartDate = DateTimeOffset.Now.AddSeconds(-currentQuestion.TimeToAnswerSeconds),
                EndDate = DateTimeOffset.Now,
                QuestionLanguage = Language.PL
            });
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
                        QuestionId = currentQuestion.QuestionId,
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

            // Stop timer when answer is submitted
            StopTimer();

            // Oznacz pytanie jako odpowiedziane przed zapisem
            var questionId = currentQuestion.QuestionId.ToString();
            answeredQuestionIds.Add(questionId);

            var isLastQuestion = currentIndex == (questions?.Count ?? 0) - 1;

            if (isUserAuthenticated)
            {
                // Zalogowany u¿ytkownik - zapisz odpowiedŸ natychmiast
                await RegisterAnswerInDatabase();

                // SprawdŸ czy to by³o ostatnie pytanie lub wszystkie pytania s¹ odpowiedziane
                var allQuestionsAnswered = answeredQuestionIds.Count >= questions!.Count;
                
                if (isLastQuestion || allQuestionsAnswered)
                {
                    Console.WriteLine($"Completing test: isLastQuestion={isLastQuestion}, allAnswered={allQuestionsAnswered}");
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
                // Niezalogowany u¿ytkownik - przechowuj odpowiedŸ lokalnie
                StoreAnswerLocally();

                if (isLastQuestion)
                {
                    // Ostatnie pytanie - zapisz ca³y test do bazy
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
                        QuestionId = currentQuestion.QuestionId,
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

            // Przechowuj odpowiedŸ lokalnie
            storedAnswers.Add(new StoredQuestionAnswer
            {
                QuestionId = currentQuestion.QuestionId,
                QuestionCategory = Enum.Parse<LicenseCategory>(Category ?? "B"),
                Question = currentQuestion.Pytanie ?? string.Empty,
                CorrectQuestionAnswer = currentQuestion.PoprawnaOdp ?? string.Empty,
                UserQuestionAnswer = selectedAnswer,
                StartDate = DateTimeOffset.Now.AddSeconds(-5), // Przybli¿ony czas rozpoczêcia
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
                // Pobierz wszystkie odpowiedzi dla tej sesji
                var answersResponse = await Http.GetAsync($"/QuestionAnswer/GetByTestSession/{testSessionId}");
                
                if (!answersResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("Failed to get answers for test completion");
                    return;
                }

                var answers = await answersResponse.Content.ReadFromJsonAsync<List<QuestionAnswer>>();
                
                if (answers == null || !answers.Any())
                {
                    Console.WriteLine("No answers found for test completion");
                    return;
                }

                // Oblicz wynik testu
                double correctAnswersCount = answers.Count(a => a.UserQuestionAnswer == a.CorrectQuestionAnswer);
                var result = (correctAnswersCount / answers.Count) * 100;

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
                // 1. Utwórz sesjê testu
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
                testSessionId = testSessionId?.Trim('"');

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

                // 3. Zakoñcz test z wynikiem
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
            
            // Jeœli kontynuujemy test, przeskocz ju¿ odpowiedziane pytania
            if (isContinuingTest)
            {
                while (currentIndex < questions!.Count && 
                       answeredQuestionIds.Contains(questions[currentIndex].QuestionId.ToString()))
                {
                    Console.WriteLine($"Skipping already answered question at index {currentIndex}");
                    currentIndex++;
                }

                // Jeœli dotarliœmy do koñca, sprawdŸ czy wszystkie pytania maj¹ odpowiedzi
                if (currentIndex >= questions.Count)
                {
                    if (answeredQuestionIds.Count >= questions.Count)
                    {
                        Console.WriteLine("All questions answered - completing test");
                        await CompleteTestInDatabase();
                        NavigateToSummary();
                        return;
                    }
                    else
                    {
                        // Nie powinno siê zdarzyæ, ale dla bezpieczeñstwa
                        Console.WriteLine($"Reached end but not all questions answered: {answeredQuestionIds.Count}/{questions.Count}");
                        await CompleteTestInDatabase();
                        NavigateToSummary();
                        return;
                    }
                }
            }

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

        public void Dispose()
        {
            StopTimer();
        }
    }

    public class StoredQuestionAnswer
    {
        public int QuestionId { get; set; }
        public LicenseCategory QuestionCategory { get; set; }
        public string Question { get; set; } = string.Empty;
        public string CorrectQuestionAnswer { get; set; } = string.Empty;
        public string UserQuestionAnswer { get; set; } = string.Empty;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public Language QuestionLanguage { get; set; } = Language.PL;
    }
}