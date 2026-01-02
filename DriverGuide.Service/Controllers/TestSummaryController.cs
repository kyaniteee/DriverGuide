using DriverGuide.Application.Queries;
using DriverGuide.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DriverGuide.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class TestSummaryController(IMediator mediator) : ControllerBase
{
    [HttpGet("GetTestSummary")]
    public async Task<ActionResult<TestSummaryDto>> GetTestSummary([FromQuery] string testSessionId)
    {
        if (string.IsNullOrWhiteSpace(testSessionId))
            return BadRequest("Test session ID is required");

        var query = new GetTestSummaryQuery { TestSessionId = testSessionId };
        var summary = await mediator.Send(query);

        if (summary == null)
            return NotFound("Test session not found");

        return Ok(summary);
    }
}