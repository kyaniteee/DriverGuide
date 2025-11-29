using DriverGuide.Domain.DTOs;
using DriverGuide.Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;

namespace DriverGuide.UI.Pages.Results
{
    public partial class Results
    {
        [Inject] private HttpClient Http { get; set; } = default!;
        [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private bool _isLoading = true;
        private bool _isAuthenticated = false;
        private string _userName = string.Empty;
        private List<TestSessionResultDto> _testResults = new();
        private string _errorMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await LoadUserTestResults();
        }

        private async Task LoadUserTestResults()
        {
            _isLoading = true;
            _errorMessage = string.Empty;

            try
            {
                var authState = await AuthStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                _isAuthenticated = user.Identity?.IsAuthenticated ?? false;

                if (!_isAuthenticated)
                {
                    _isLoading = false;
                    return;
                }

                _userName = user.Identity?.Name ?? "Użytkownik";
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    _errorMessage = "Nie można odnaleźć identyfikatora użytkownika";
                    _isLoading = false;
                    return;
                }

                var sessionsResponse = await Http.GetAsync($"/TestSession/GetByUserId/{userId}");

                if (!sessionsResponse.IsSuccessStatusCode)
                {
                    _errorMessage = "Nie udało się pobrać wyników testów";
                    _isLoading = false;
                    return;
                }

                var sessions = await sessionsResponse.Content.ReadFromJsonAsync<List<TestSession>>();

                if (sessions == null || !sessions.Any())
                {
                    _isLoading = false;
                    return;
                }

                foreach (var session in sessions)
                {
                    var answersResponse = await Http.GetAsync($"/QuestionAnswer/GetByTestSession/{session.TestSessionId}");

                    int correctAnswers = 0;
                    int totalQuestions = 0;

                    if (answersResponse.IsSuccessStatusCode)
                    {
                        var answers = await answersResponse.Content.ReadFromJsonAsync<List<QuestionAnswer>>();

                        if (answers != null && answers.Any())
                        {
                            correctAnswers = answers.Count(a => a.UserQuestionAnswer == a.CorrectQuestionAnswer);
                            totalQuestions = answers.Count;
                        }
                    }

                    _testResults.Add(new TestSessionResultDto
                    {
                        TestSessionId = session.TestSessionId ?? string.Empty,
                        Result = session.Result,
                        StartDate = session.StartDate,
                        EndDate = session.EndDate,
                        TotalQuestions = totalQuestions,
                        CorrectAnswers = correctAnswers
                    });
                }

                _testResults = _testResults.OrderByDescending(r => r.StartDate).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading test results: {ex.Message}");
                _errorMessage = "Wystąpił błąd podczas ładowania wyników";
            }
            finally
            {
                _isLoading = false;
            }
        }

        private void ViewDetails(string testSessionId)
        {
            Navigation.NavigateTo($"/test-summary/{testSessionId}");
        }

        private void GoToProfile()
        {
            Navigation.NavigateTo("/profile");
        }

        private void StartNewTest()
        {
            Navigation.NavigateTo("/test");
        }

        private void ContinueTest(string testSessionId)
        {
            Navigation.NavigateTo($"/continue-test/{testSessionId}");
        }

        private string GetResultClass(TestSessionResultDto result)
        {
            if (!result.IsCompleted) return "result-incomplete";
            return result.IsPassed ? "result-passed" : "result-failed";
        }

        private string GetResultBadgeClass(TestSessionResultDto result)
        {
            if (!result.IsCompleted) return "badge-incomplete";
            return result.IsPassed ? "badge-success" : "badge-danger";
        }

        private string GetResultText(TestSessionResultDto result)
        {
            if (!result.IsCompleted) return "Nieukończony";
            return result.IsPassed ? "Zaliczony" : "Niezaliczony";
        }

        private int GetIncompleteCount()
        {
            return _testResults.Count(r => !r.IsCompleted);
        }

        private int GetCompletedCount()
        {
            return _testResults.Count(r => r.IsCompleted);
        }

        private int GetPassedCount()
        {
            return _testResults.Count(r => r.IsPassed);
        }

        private int GetFailedCount()
        {
            return _testResults.Count(r => r.IsFailed);
        }
    }
}
