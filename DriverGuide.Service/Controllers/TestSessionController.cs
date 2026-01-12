using DriverGuide.Application.Commands;
using DriverGuide.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DriverGuide.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class TestSessionController(IMediator mediator) : ControllerBase
{
    [HttpPost("Create")]
    public async Task<IActionResult> CreateTestSession([FromBody] CreateTestSessionCommand command)
    {
        var sessionId = await mediator.Send(command);
        return Ok(sessionId);
    }

    [HttpPost("Complete")]
    public async Task<IActionResult> CompleteTestSession([FromBody] CompleteTestSessionCommand command)
    {
        var result = await mediator.Send(command);
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