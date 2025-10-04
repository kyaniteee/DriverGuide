using DriverGuide.Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace DriverGuide.UI.Pages.Quiz
{
    public partial class Quiz
    {
        [Parameter]
        public string? Category { get; set; }

        private double questionsCount => questions?.Count ?? 32;

        private string? fileBlobUrl;
        private int currentIndex = 0;
        private string? selectedAnswer;
        private bool isVideoLoading = true;
        private bool isLoadingQuestion = false;

        private Question? currentQuestion;
        private readonly List<Question>? questions = [];
        private readonly List<QuestionFile>? questionsFiles = [];

        protected override async Task OnInitializedAsync()
        {
            isLoadingQuestion = true;
            var questionsHttpResponse = await Http.GetAsync($"/Question/GetQuizQuestions?category={Category}");
            if (!questionsHttpResponse.IsSuccessStatusCode)
                return;

            var questionsResponse = await questionsHttpResponse.Content.ReadFromJsonAsync<List<Question>>();
            if (questionsResponse is not null && questionsResponse.Any())
                questions!.AddRange(questionsResponse);

            // Just load the first question, not all media files
            await LoadQuestionAsync();
        }

        private async Task LoadQuestionAsync()
        {
            if (questions is null || currentIndex >= questions.Count)
                return;

            isLoadingQuestion = true;
            StateHasChanged(); // Update UI to show loading state

            selectedAnswer = null;
            currentQuestion = questions[currentIndex];

            // Clean up previous blob URL if exists
            if (!string.IsNullOrWhiteSpace(fileBlobUrl))
            {
                try
                {
                    await JS.InvokeVoidAsync("revokeObjectURL", fileBlobUrl);
                }
                catch { /* Ignore errors on cleanup */ }
                fileBlobUrl = null;
            }

            // Only fetch media if needed for this question
            if (!string.IsNullOrWhiteSpace(currentQuestion.Media))
            {
                await LoadMediaForCurrentQuestion();
            }

            // Small delay to ensure the UI updates correctly
            await Task.Delay(500);
            isLoadingQuestion = false;
            StateHasChanged();
        }

        private async Task LoadMediaForCurrentQuestion()
        {
            if (currentQuestion == null || string.IsNullOrWhiteSpace(currentQuestion.Media))
                return;

            // Check if we already have this file cached
            var cachedFile = questionsFiles?.Find(x => x.Name == currentQuestion.Media);
            if (cachedFile != null)
            {
                await CreateBlobUrl(cachedFile);
                return;
            }

            // If not cached, fetch it
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

            // Verify we have actual data
            if (questionFile.File == null || questionFile.File.Length == 0)
            {
                Console.WriteLine("QuestionFile has no data");
                return;
            }

            // Cache it for future use
            questionsFiles!.Add(questionFile);
            await CreateBlobUrl(questionFile);
        }

        private async Task CreateBlobUrl(QuestionFile file)
        {
            if (file?.File == null || string.IsNullOrWhiteSpace(file.ContentType))
            {
                Console.WriteLine("Cannot create blob URL: missing file data or content type");
                return;
            }

            try
            {
                // Create the blob URL using our JavaScript function
                fileBlobUrl = await JS.InvokeAsync<string>("createObjectURL", file.File, file.ContentType);

                // Log successful creation for debugging
                Console.WriteLine($"Successfully created blob URL for {file.Name} ({file.ContentType})");

                // For video files, we just need a small delay to let the browser process the video
                var mimeType = !string.IsNullOrEmpty(currentQuestion?.Media) ?
                    Utils.FileExtensionUtils.GetMimeType(currentQuestion.Media) : string.Empty;

                if (mimeType.StartsWith("video/"))
                {
                    // Wait a moment for the browser to process the video
                    await Task.Delay(200);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating blob URL: {ex.Message}");
                fileBlobUrl = null;
            }
        }

        private async Task SubmitAnswer()
        {
            if (selectedAnswer != null && currentQuestion != null && !isLoadingQuestion)
                await NextQuestion();
        }

        private async Task NextQuestion()
        {
            if (isLoadingQuestion)
                return;

            // Increment the index for the next question
            currentIndex++;

            // Reset to first question if we've gone beyond the end
            if (currentIndex >= questions!.Count)
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
        // Add this method to handle answer selection
        private void SelectAnswer(string answer)
        {
            selectedAnswer = answer;
            StateHasChanged(); // Force re-render to update button style
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
}