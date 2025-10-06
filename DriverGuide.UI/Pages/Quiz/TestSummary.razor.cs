using DriverGuide.Domain.Models;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace DriverGuide.UI.Pages.Quiz
{
    public partial class TestSummary
    {
        [Parameter] public string TestSessionId { get; set; } = string.Empty;

        [Inject] private HttpClient Http { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private bool _isLoading = true;
        private TestSession? _testSession;
        private List<QuestionAnswer> _questionAnswers = new();
        private Dictionary<string, Question> _questions = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                // Pobierz dane sesji testu
                var sessionResponse = await Http.GetAsync($"/TestSession/{TestSessionId}");
                if (sessionResponse.IsSuccessStatusCode)
                {
                    _testSession = await sessionResponse.Content.ReadFromJsonAsync<TestSession>();
                }

                // Pobierz odpowiedzi z sesji
                var answersResponse = await Http.GetAsync($"/QuestionAnswer/GetByTestSession/{TestSessionId}");
                if (answersResponse.IsSuccessStatusCode)
                {
                    _questionAnswers = await answersResponse.Content.ReadFromJsonAsync<List<QuestionAnswer>>() ?? new List<QuestionAnswer>();
                }

                // Pobierz pytania dla dodatkowych informacji
                if (_questionAnswers.Any())
                {
                    foreach (var answer in _questionAnswers)
                    {
                        if (!string.IsNullOrEmpty(answer.QuestionId) && int.TryParse(answer.QuestionId, out int questionId))
                        {
                            var questionResponse = await Http.GetAsync($"/Question/{questionId}");
                            if (questionResponse.IsSuccessStatusCode)
                            {
                                var question = await questionResponse.Content.ReadFromJsonAsync<Question>();
                                if (question != null)
                                {
                                    _questions[answer.QuestionId] = question;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading test data: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
            }
        }

        private string GetCategoryName()
        {
            if (_questionAnswers.FirstOrDefault() is QuestionAnswer firstAnswer)
            {
                var category = firstAnswer.QuestionCategory;
                var field = category.GetType().GetField(category.ToString());
                var attr = field?.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
                return attr?.Name ?? category.ToString();
            }
            return "Nieznana kategoria";
        }

        private string GetTestDuration()
        {
            if (_testSession?.EndDate == null || _testSession.StartDate == default)
                return "n/a";

            var duration = _testSession.EndDate.Value - _testSession.StartDate;
            return $"{(int)duration.TotalMinutes:D2}:{duration.Seconds:D2}";
        }

        private string GetAnswerTime(QuestionAnswer answer)
        {
            if (answer.EndDate == null || answer.StartDate == default)
                return "n/a";

            var duration = answer.EndDate.Value - answer.StartDate;
            return $"{duration.TotalSeconds:F1} s";
        }

        private int GetCorrectAnswersCount()
        {
            return _questionAnswers.Count(a => a.UserQuestionAnswer == a.CorrectQuestionAnswer);
        }

        private string GetPercentage()
        {
            if (_questionAnswers.Count == 0)
                return "0";

            double percentage = (double)GetCorrectAnswersCount() / _questionAnswers.Count * 100;
            return percentage.ToString("F0");
        }

        private bool IsTestPassed()
        {
            return double.Parse(GetPercentage()) >= 68; // Próg zaliczenia 68%
        }

        private int GetMissingPoints()
        {
            if (IsTestPassed()) return 0;

            var correctCount = GetCorrectAnswersCount();
            var minimumNeeded = (int)Math.Ceiling(_questionAnswers.Count * 0.68);
            return minimumNeeded - correctCount;
        }

        private string GetMissingPointsText()
        {
            var missingPoints = GetMissingPoints();
            if (missingPoints == 1) return "odpowiedzi do zaliczenia";
            if (missingPoints > 1 && missingPoints < 5) return "odpowiedzi do zaliczenia";
            return "odpowiedzi do zaliczenia";
        }

        private int GetQuestionPoints(string? questionId)
        {
            if (questionId != null && _questions.TryGetValue(questionId, out var question))
            {
                return question.Points;
            }
            return 1;
        }

        private string GetResultClass()
        {
            return IsTestPassed() ? "result-passed" : "result-failed";
        }

        private bool IsAnswerCorrect(QuestionAnswer answer)
        {
            return answer.UserQuestionAnswer == answer.CorrectQuestionAnswer;
        }

        private string GetAnswerClass(string answerOption, QuestionAnswer answer)
        {
            if (answerOption == answer.CorrectQuestionAnswer)
                return "correct";
            else if (answerOption == answer.UserQuestionAnswer && answerOption != answer.CorrectQuestionAnswer)
                return "incorrect";
            else
                return "";
        }

        private List<string> GetAnswerOptions(Question question)
        {
            List<string> options = new();

            if (!string.IsNullOrEmpty(question.OdpowiedzA) &&
                !string.IsNullOrEmpty(question.OdpowiedzB) &&
                !string.IsNullOrEmpty(question.OdpowiedzC))
            {
                options.Add(question.OdpowiedzA);
                options.Add(question.OdpowiedzB);
                options.Add(question.OdpowiedzC);
            }
            else
            {
                options.Add("Tak");
                options.Add("Nie");
            }

            return options;
        }

        private void GoToHome()
        {
            Navigation.NavigateTo("/");
        }

        private void RetakeTest()
        {
            var category = _questionAnswers.FirstOrDefault()?.QuestionCategory.ToString() ?? "B";
            Navigation.NavigateTo($"/quiz/{category}");
        }
    }
}