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

    [HttpGet(nameof(GetQuestionsByCategories), Name = nameof(GetQuestionsByCategories))]
    public async Task<ActionResult<IEnumerable<Question>>> GetQuestionsByCategories(int quantity, [FromQuery] string[] categories)
    {
        if (categories is null || !categories.Any())
            return BadRequest();

        var cat = categories.Select(x => Enum.Parse<LicenseCategory>(x, true)).ToArray();
        var result = await questionRepository.GetRandomQuestionsQuantityByCategories(quantity, cat);
        if (result == null)
            return NotFound();

        return Ok(result);
    }
}
