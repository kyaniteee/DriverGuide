using DriverGuide.Application.Requests;
using DriverGuide.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DriverGuide.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class TestSessionController : ControllerBase
{
    private readonly ITestSessionRepository _testSessionRepository;
    private readonly IQuestionAnswerRepository _questionAnswerRepository;

    public TestSessionController(
        ITestSessionRepository testSessionRepository,
        IQuestionAnswerRepository questionAnswerRepository)
    {
        _testSessionRepository = testSessionRepository;
        _questionAnswerRepository = questionAnswerRepository;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateTestSession([FromBody] CreateTestSessionRequest request)
    {
        var session = await _testSessionRepository.CreateSessionAsync(request.Category, request.StartDate, request.UserId);
        return Ok(session.TestSessionId);
    }

    [HttpPost("Complete")]
    public async Task<IActionResult> CompleteTestSession([FromBody] CompleteTestSessionRequest request)
    {
        var result = await _testSessionRepository.CompleteSessionAsync(request.TestSessionId, request.Result);
        return result ? Ok() : NotFound("Test session not found");
    }

    [HttpGet("{testSessionId}")]
    public async Task<IActionResult> GetTestSession(string testSessionId)
    {
        var session = await _testSessionRepository.GetByIdAsync(testSessionId);
        return session != null ? Ok(session) : NotFound();
    }

    [HttpGet("GetByUserId/{userId}")]
    public async Task<IActionResult> GetUserTestSessions(Guid userId)
    {
        var allSessions = await _testSessionRepository.FindAsync(ts => ts.UserId == userId);
        var sortedSessions = allSessions.OrderByDescending(s => s.StartDate).ToList();
        return Ok(sortedSessions);
    }

    [HttpGet("Details/{testSessionId}")]
    public async Task<IActionResult> GetTestSessionDetails(string testSessionId)
    {
        var session = await _testSessionRepository.GetByIdAsync(testSessionId);
        if (session == null)
            return NotFound("Test session not found");

        var answers = await _questionAnswerRepository.GetByTestSessionIdAsync(testSessionId);

        return Ok(new
        {
            session.TestSessionId,
            session.StartDate,
            session.EndDate,
            session.Result,
            session.UserId,
            TotalAnswers = answers?.Count ?? 0,
            AnsweredQuestions = answers?.Count(a => !string.IsNullOrEmpty(a.UserQuestionAnswer)) ?? 0
        });
    }
}