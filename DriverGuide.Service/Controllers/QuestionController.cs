using DriverGuide.Application.Queries;
using DriverGuide.Domain.Enums;
using DriverGuide.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DriverGuide.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionController(IMediator mediator, ILogger<QuestionController> logger) : ControllerBase
{
    [HttpGet("{questionId}", Name = nameof(GetQuestion))]
    public async Task<ActionResult<Question>> GetQuestion([Required] int questionId)
    {
        var query = new GetQuestionQuery { QuestionId = questionId };
        var result = await mediator.Send(query);
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet(nameof(GetQuestions), Name = nameof(GetQuestions))]
    public async Task<ActionResult<IEnumerable<Question>>> GetQuestions()
    {
        var query = new GetAllQuestionsQuery();
        var result = await mediator.Send(query);
        
        if (result == null || result.Count == 0)
            return NotFound();

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet(nameof(GetQuizQuestions), Name = nameof(GetQuizQuestions))]
    public async Task<ActionResult<IEnumerable<Question>>> GetQuizQuestions([FromQuery] string category)
    {
        if (string.IsNullOrWhiteSpace(category))
            return BadRequest("Nie podano kategorii prawa jazdy!");

        if (!Enum.TryParse<LicenseCategory>(category, true, out var licenseCategory))
            return BadRequest("Nieprawidłowa kategoria prawa jazdy!");

        var query = new GetQuizQuestionsQuery { Category = licenseCategory };
        var result = await mediator.Send(query);
        
        if (result == null || result.Count == 0)
            return NotFound();

        return Ok(result);
    }
}
