using DriverGuide.Application.Queries;
using DriverGuide.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DriverGuide.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class TestSessionController(IMediator mediator) : ControllerBase
{
    [HttpPost("Create")]
    public async Task<IActionResult> CreateTestSession([FromBody] CreateTestSessionRequest request)
    {
        var sessionId = await mediator.Send(request);
        return Ok(sessionId);
    }

    [HttpPost("Complete")]
    public async Task<IActionResult> CompleteTestSession([FromBody] CompleteTestSessionRequest request)
    {
        var result = await mediator.Send(request);
        return result ? Ok() : NotFound("Test session not found");
    }

    [HttpGet("{testSessionId}")]
    public async Task<IActionResult> GetTestSession(string testSessionId)
    {
        var query = new GetTestSessionQuery { TestSessionId = testSessionId };
        var session = await mediator.Send(query);
        
        return session != null ? Ok(session) : NotFound();
    }

    [HttpGet("GetByUserId/{userId}")]
    public async Task<IActionResult> GetUserTestSessions(Guid userId)
    {
        var query = new GetUserTestSessionsQuery { UserId = userId };
        var sortedSessions = await mediator.Send(query);
        
        return Ok(sortedSessions);
    }

    [HttpGet("Details/{testSessionId}")]
    public async Task<IActionResult> GetTestSessionDetails(string testSessionId)
    {
        var query = new GetTestSessionDetailsQuery { TestSessionId = testSessionId };
        var details = await mediator.Send(query);
        
        if (details == null)
            return NotFound("Test session not found");

        return Ok(details);
    }
}