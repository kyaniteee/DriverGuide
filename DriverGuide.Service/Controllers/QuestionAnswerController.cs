using DriverGuide.Application.Commands;
using DriverGuide.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DriverGuide.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionAnswerController(IMediator mediator, ILogger<QuestionAnswerController> logger) : ControllerBase
{
    [HttpPost("StartQuestion")]
    public async Task<IActionResult> StartQuestion([FromBody] StartQuestionCommand command)
    {
        try
        {
            var questionAnswerId = await mediator.Send(command);
            return Ok(questionAnswerId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error starting question");
            return StatusCode(500, "An error occurred while starting the question");
        }
    }

    [HttpPost("SubmitAnswer")]
    public async Task<IActionResult> SubmitAnswer([FromBody] SubmitAnswerCommand command)
    {
        try
        {
            await mediator.Send(command);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Invalid operation while submitting answer");
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error submitting answer");
            return StatusCode(500, "An error occurred while submitting the answer");
        }
    }

    [HttpGet("GetByTestSession/{testSessionId}")]
    public async Task<IActionResult> GetByTestSession(string testSessionId)
    {
        var query = new GetQuestionAnswersByTestSessionQuery { TestSessionId = testSessionId };
        var answers = await mediator.Send(query);

        return Ok(answers);
    }

    [HttpPost("BulkSubmitAnswers")]
    public async Task<IActionResult> BulkSubmitAnswers([FromBody] BulkAnswersCommand command)
    {
        try
        {
            await mediator.Send(command);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing bulk answers");
            return StatusCode(500, "An error occurred while processing the answers");
        }
    }
}
