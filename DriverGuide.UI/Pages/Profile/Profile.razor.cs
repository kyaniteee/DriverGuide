using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;
using DriverGuide.Domain.Models;

namespace DriverGuide.UI.Pages.Profile
{
    public partial class Profile
    {
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private string userName = string.Empty;
        private string userEmail = string.Empty;
        private string firstName = string.Empty;
        private string lastName = string.Empty;
        private string birthDate = string.Empty;
        private List<string> userRoles = new();

        // Statystyki podstawowe
        private int _completedTests = 0;
        private int _totalTests = 0;
        private double _averageScore = 0;
        private double _totalStudyHours = 0;
        private bool _isLoadingStats = true;

        // Statystyki rozszerzone
        private int _passedTests = 0;
        private int _failedTests = 0;
        private int _incompleteTests = 0;
        private double _bestScore = 0;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                userName = user.Identity.Name ?? "U¿ytkownik";
                userEmail = user.FindFirst(ClaimTypes.Email)?.Value ?? user.FindFirst("email")?.Value ?? "Brak";
                firstName = user.FindFirst("FirstName")?.Value ?? "Brak";
                lastName = user.FindFirst("LastName")?.Value ?? "Brak";
                birthDate = user.FindFirst("BirthDate")?.Value ?? "Brak";
                userRoles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

                if (!userRoles.Any())
                {
                    userRoles.Add("User");
                }

                // Za³aduj statystyki
                await LoadUserStatistics(user);
            }
        }

        private async Task LoadUserStatistics(ClaimsPrincipal user)
        {
            _isLoadingStats = true;

            try
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    Console.WriteLine($"Loading statistics for user: {userId}");

                    // Pobierz wszystkie sesje testowe u¿ytkownika
                    var sessionsResponse = await Http.GetAsync($"/TestSession/GetByUserId/{userId}");

                    if (sessionsResponse.IsSuccessStatusCode)
                    {
                        var sessions = await sessionsResponse.Content.ReadFromJsonAsync<List<TestSession>>();

                        if (sessions != null && sessions.Any())
                        {
                            _totalTests = sessions.Count;
                            _completedTests = sessions.Count(s => s.EndDate.HasValue && s.Result.HasValue);
                            _incompleteTests = sessions.Count(s => !s.EndDate.HasValue || !s.Result.HasValue);

                            // Oblicz statystyki tylko z ukoñczonych testów
                            var completedSessions = sessions.Where(s => s.EndDate.HasValue && s.Result.HasValue).ToList();
                            
                            if (completedSessions.Any())
                            {
                                _averageScore = completedSessions.Average(s => s.Result ?? 0);
                                _bestScore = completedSessions.Max(s => s.Result ?? 0);
                                
                                // Zaliczone (?68%) i niezaliczone (<68%)
                                _passedTests = completedSessions.Count(s => (s.Result ?? 0) >= 68);
                                _failedTests = completedSessions.Count(s => (s.Result ?? 0) < 68);
                            }

                            // Oblicz ca³kowity czas nauki (w godzinach)
                            foreach (var session in completedSessions)
                            {
                                if (session.EndDate.HasValue)
                                {
                                    var duration = session.EndDate.Value - session.StartDate;
                                    _totalStudyHours += duration.TotalHours;
                                }
                            }

                            Console.WriteLine($"? User statistics loaded:");
                            Console.WriteLine($"  Total tests: {_totalTests}");
                            Console.WriteLine($"  Completed: {_completedTests}");
                            Console.WriteLine($"  Passed: {_passedTests}");
                            Console.WriteLine($"  Failed: {_failedTests}");
                            Console.WriteLine($"  Incomplete: {_incompleteTests}");
                            Console.WriteLine($"  Average score: {_averageScore:F1}%");
                            Console.WriteLine($"  Best score: {_bestScore:F1}%");
                            Console.WriteLine($"  Study time: {_totalStudyHours:F1}h");
                        }
                        else
                        {
                            Console.WriteLine("? No test sessions found for user");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"? Failed to load user sessions: {sessionsResponse.StatusCode}");
                    }
                }
                else
                {
                    Console.WriteLine("? User ID claim not found or invalid");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Error loading user statistics: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
            finally
            {
                _isLoadingStats = false;
            }
        }

        private string GetAverageScoreDisplay()
        {
            if (_completedTests == 0)
                return "0%";
            
            return $"{_averageScore:F0}%";
        }

        private string GetBestScoreDisplay()
        {
            if (_completedTests == 0)
                return "0%";
            
            return $"{_bestScore:F0}%";
        }

        private string GetStudyHoursDisplay()
        {
            if (_totalStudyHours < 0.1)
                return "0";
            
            return $"{_totalStudyHours:F1}";
        }

        private string GetPassRateDisplay()
        {
            if (_completedTests == 0)
                return "0%";
            
            double passRate = ((double)_passedTests / _completedTests) * 100;
            return $"{passRate:F0}%";
        }

        // Metody nawigacji
        private void StartNewTest()
        {
            Navigation.NavigateTo("/test");
        }

        private void ViewResults()
        {
            Navigation.NavigateTo("/wyniki");
        }
    }
}
