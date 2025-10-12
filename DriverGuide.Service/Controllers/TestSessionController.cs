using DriverGuide.Application.Requests;
using DriverGuide.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DriverGuide.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class TestSessionController : ControllerBase
{
    private readonly ITestSessionRepository _testSessionRepository;

    public TestSessionController(ITestSessionRepository testSessionRepository)
    {
        _testSessionRepository = testSessionRepository;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateTestSession([FromBody] CreateTestSessionRequest request)
    {
        var session = await _testSessionRepository.CreateSessionAsync(request.Category, request.StartDate);
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
}