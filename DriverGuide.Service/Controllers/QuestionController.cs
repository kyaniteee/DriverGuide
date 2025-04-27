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
}
