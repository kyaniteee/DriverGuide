using DriverGuide.Domain.Enums;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DriverGuide.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionController(IQuestionRepository questionRepository, ILogger<QuestionController> logger) : ControllerBase
{
    [HttpGet("{questionId}", Name = nameof(GetQuestion))]
    public async Task<ActionResult<Question>> GetQuestion([Required] int questionId)
    {
        var result = await questionRepository.GetByIdAsync(questionId);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet(nameof(GetQuestions), Name = nameof(GetQuestions))]
    public async Task<ActionResult<IEnumerable<Question>>> GetQuestions()
    {
        var result = await questionRepository.GetAllAsync();
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet(nameof(GetQuizQuestions), Name = nameof(GetQuizQuestions))]
    public async Task<ActionResult<IEnumerable<Question>>> GetQuizQuestions([FromQuery] string category)
    {
        if (string.IsNullOrWhiteSpace(category))
            return BadRequest("Nie podano kategorii prawa jazdy!");

        var cat = Enum.Parse<LicenseCategory>(category, true);
        var result = await questionRepository.GetQuizQuestions(cat);
        if (result == null)
            return NotFound();

        return Ok(result);
    }
}
