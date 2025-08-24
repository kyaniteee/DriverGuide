using DriverGuide.Domain.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace DriverGuide.UI.Pages.Quiz
{
    public partial class Quiz
    {
        [Parameter]
        public string? Category { get; set; }

        private double questionsCount => questions?.Count ?? 32;
        private double ProgressPercent => ((double)(currentIndex + 1) / questionsCount) * 100;

        private string? fileBlobUrl;
        private int currentIndex = 0;
        private string? selectedAnswer;

        private Question? currentQuestion;
        private readonly List<Question>? questions = [];
        private readonly List<QuestionFile>? questionsFiles = [];

        protected override async Task OnInitializedAsync()
        {
            var questionsHttpResponse = await Http.GetAsync($"/Question/GetQuizQuestions?category={Category}");
            if (!questionsHttpResponse.IsSuccessStatusCode)
                return;

            var questionsResponse = await questionsHttpResponse.Content.ReadFromJsonAsync<List<Question>>();
            if (questionsResponse is not null && questionsResponse.Any())
                questions!.AddRange(questionsResponse);

            var questionFileNames = questions!.Where(x => !string.IsNullOrWhiteSpace(x.Media)).Select(x => x.Media).GroupBy(x => x).Select(x => x.Key).ToList();
            foreach (var fileName in questionFileNames)
            {
                var fileResponse = await Http.GetAsync($"/QuestionFile/GetQuestionFileByName?questionFileName={fileName}");
                if (!fileResponse.IsSuccessStatusCode)
                    continue;

                var questionFile = await fileResponse.Content.ReadFromJsonAsync<QuestionFile>();
                if (questionFile is not null)
                    questionsFiles!.Add(questionFile);
            }

            await NextQuestion();
        }

        private async Task LoadQuestionAsync()
        {
            if (questions is null || currentIndex > questions.Count)
                return;

            selectedAnswer = null;
            currentQuestion = questions[currentIndex];

            await SetFileUrl();

            StateHasChanged();
        }

        private async Task SetFileUrl()
        {
            fileBlobUrl = string.Empty;

            if (string.IsNullOrWhiteSpace(currentQuestion?.Media))
                return;

            var file = questionsFiles?.Find(x => x.Name == currentQuestion.Media);
            if (file is null)
                return;

            fileBlobUrl = await JS.InvokeAsync<string>("createObjectURL", new object[] { file.File!, file.ContentType! });
        }

        private async Task SubmitAnswer()
        {
            if (selectedAnswer != null && currentQuestion != null)
                await NextQuestion();
        }

        private async Task NextQuestion()
        {
            currentIndex++;
            if (currentIndex > questions!.Count)
                currentIndex = 0;

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
    }
}
