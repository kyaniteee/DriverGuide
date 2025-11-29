using DriverGuide.Domain.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace DriverGuide.UI.Pages.ContinueTest
{
    public partial class ContinueTest
    {
        [Parameter]
        public string? TestSessionId { get; set; }

        private bool _isLoading = true;
        private string _errorMessage = string.Empty;
        private TestSession? _testSession;
        private string _category = string.Empty;
        private int _answeredQuestions = 0;
        private int _totalQuestions = 32;
        private int _progressPercentage = 0;

        protected override async Task OnInitializedAsync()
        {
            await LoadTestSessionInfo();
        }

        private async Task LoadTestSessionInfo()
        {
            _isLoading = true;
            _errorMessage = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(TestSessionId))
                {
                    _errorMessage = "Nieprawid³owy identyfikator sesji testowej";
                    _isLoading = false;
                    return;
                }

                // Pobierz szczegó³y sesji testowej
                var sessionResponse = await Http.GetAsync($"/TestSession/{TestSessionId}");

                if (!sessionResponse.IsSuccessStatusCode)
                {
                    _errorMessage = "Nie mo¿na odnaleŸæ sesji testowej";
                    _isLoading = false;
                    return;
                }

                _testSession = await sessionResponse.Content.ReadFromJsonAsync<TestSession>();

                if (_testSession == null)
                {
                    _errorMessage = "Nie mo¿na za³adowaæ danych sesji testowej";
                    _isLoading = false;
                    return;
                }

                // Jeœli test jest ju¿ ukoñczony, przekieruj do podsumowania
                if (_testSession.EndDate.HasValue)
                {
                    Navigation.NavigateTo($"/test-summary/{TestSessionId}");
                    return;
                }

                // Pobierz odpowiedzi dla tej sesji
                var answersResponse = await Http.GetAsync($"/QuestionAnswer/GetByTestSession/{TestSessionId}");

                if (answersResponse.IsSuccessStatusCode)
                {
                    var answers = await answersResponse.Content.ReadFromJsonAsync<List<QuestionAnswer>>();

                    if (answers != null && answers.Any())
                    {
                        // Zlicz tylko pytania z faktyczn¹ odpowiedzi¹ (UserQuestionAnswer wype³niony)
                        _answeredQuestions = answers.Count(a => !string.IsNullOrEmpty(a.UserQuestionAnswer));
                        
                        // Pobierz kategoriê z pierwszej odpowiedzi
                        _category = answers.First().QuestionCategory.ToString();
                        
                        Console.WriteLine($"Test progress: {_answeredQuestions}/{_totalQuestions} questions answered");
                    }
                    else
                    {
                        // Brak odpowiedzi - test dopiero rozpoczêty
                        _answeredQuestions = 0;
                        Console.WriteLine("No answers found - test just started");
                    }
                }
                else
                {
                    Console.WriteLine("Failed to load answers");
                }

                // Oblicz procent postêpu
                _progressPercentage = _totalQuestions > 0 
                    ? (int)((double)_answeredQuestions / _totalQuestions * 100) 
                    : 0;

                Console.WriteLine($"Progress: {_progressPercentage}% ({_answeredQuestions}/{_totalQuestions})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading test session info: {ex.Message}");
                _errorMessage = "Wyst¹pi³ b³¹d podczas ³adowania informacji o teœcie";
            }
            finally
            {
                _isLoading = false;
            }
        }

        private void StartContinuation()
        {
            if (!string.IsNullOrEmpty(_category) && !string.IsNullOrEmpty(TestSessionId))
            {
                Navigation.NavigateTo($"/quiz/{_category}/{TestSessionId}");
            }
            else
            {
                _errorMessage = "Nie mo¿na kontynuowaæ testu - brak wymaganych danych";
            }
        }

        private void GoToResults()
        {
            Navigation.NavigateTo("/wyniki");
        }
    }
}
