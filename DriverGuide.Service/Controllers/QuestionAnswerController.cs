using DriverGuide.Application.Requests;
using DriverGuide.Domain.Enums;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace DriverGuide.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionAnswerController : ControllerBase
{
    private readonly IQuestionAnswerRepository _questionAnswerRepository;
    private readonly ILogger<QuestionAnswerController> _logger;

    public QuestionAnswerController(IQuestionAnswerRepository questionAnswerRepository, ILogger<QuestionAnswerController> logger)
    {
        _questionAnswerRepository = questionAnswerRepository;
        _logger = logger;
    }

    [HttpPost("StartQuestion")]
    public async Task<IActionResult> StartQuestion([FromBody] StartQuestionRequest request)
    {
        var questionAnswer = new QuestionAnswer
        {
            QuestionAnswerId = Guid.NewGuid().ToString(),
            TestSessionId = request.TestSessionId,
            QuestionId = request.QuestionId,
            QuestionCategory = request.QuestionCategory,
            Question = request.Question,
            CorrectQuestionAnswer = request.CorrectQuestionAnswer,
            StartDate = request.StartDate,
            QuestionLanguage = request.QuestionLanguage
        };

        await _questionAnswerRepository.CreateAsync(questionAnswer);
        return Ok(questionAnswer.QuestionAnswerId);
    }

    [HttpPost("SubmitAnswer")]
    public async Task<IActionResult> SubmitAnswer([FromBody] SubmitAnswerRequest request)
    {
        // Znajdź wszystkie wpisy dotyczące tego pytania w tej sesji
        var answers = await _questionAnswerRepository.GetByTestSessionIdAsync(request.TestSessionId);
        var answer = answers.LastOrDefault(a => a.QuestionId == request.QuestionId && a.EndDate == null);

        if (answer == null)
        {
            _logger.LogWarning($"No open question found for TestSession {request.TestSessionId}, Question {request.QuestionId}");
            return NotFound("No open question found");
        }

        answer.UserQuestionAnswer = request.UserAnswer;
        answer.EndDate = request.EndDate ?? DateTimeOffset.Now;

        await _questionAnswerRepository.UpdateAsync(answer);
        return Ok();
    }

    [HttpGet("GetByTestSession/{testSessionId}")]
    public async Task<IActionResult> GetByTestSession(string testSessionId)
    {
        var answers = await _questionAnswerRepository.GetByTestSessionIdAsync(testSessionId);
        return Ok(answers);
    }

    [HttpPost("BulkSubmitAnswers")]
    public async Task<IActionResult> BulkSubmitAnswers([FromBody] BulkAnswersRequest request)
    {
        if (request.Answers == null || !request.Answers.Any())
        {
            return BadRequest("No answers provided");
        }

        if (string.IsNullOrEmpty(request.TestSessionId))
        {
            return BadRequest("TestSessionId is required");
        }

        try
        {
            foreach (var answer in request.Answers)
            {
                var questionAnswer = new QuestionAnswer
                {
                    QuestionAnswerId = Guid.NewGuid().ToString(),
                    TestSessionId = request.TestSessionId,
                    QuestionId = answer.QuestionId,
                    QuestionCategory = answer.QuestionCategory,
                    Question = answer.Question,
                    CorrectQuestionAnswer = answer.CorrectQuestionAnswer,
                    UserQuestionAnswer = answer.UserQuestionAnswer,
                    StartDate = answer.StartDate,
                    EndDate = answer.EndDate,
                    QuestionLanguage = answer.QuestionLanguage
                };

                await _questionAnswerRepository.CreateAsync(questionAnswer);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing bulk answers");
            return StatusCode(500, "An error occurred while processing the answers");
        }
    }
}
