using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DriverGuide.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionAnswerController(IQuestionAnswerRepository questionAnswerReposiotory, ILogger<QuestionAnswerController> logger) : ControllerBase
{
    [HttpGet("{questionAnswerId}", Name = nameof(GetQuestionAnswer))]
    public async Task<ActionResult<QuestionAnswer>> GetQuestionAnswer([Required] int questionAnswerId)
    {
        var result = await questionAnswerReposiotory.GetByIdAsync(questionAnswerId);
        if (result == null)
            return NotFound();

        return Ok(result);
    }
}
